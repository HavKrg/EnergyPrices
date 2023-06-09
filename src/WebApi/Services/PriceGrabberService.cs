using System.Text.Json;
using Application.DTOs.Area;
using Application.DTOs.HourlyPrices;
using Application.Interfaces;

namespace WebApi.Services;


public class PriceGrabberService : BackgroundService
{
    private readonly IDailyPricesService _dailyPricesService;
    private readonly IAreaService _areaService;
    private readonly HttpClient _httpClient;

    private readonly string _requestUri =
        "https://www.nordpoolgroup.com/api/marketdata/page/10?currency=NOK,NOK,EUR,EUR";

    public PriceGrabberService(IDailyPricesService dailyPricesService, IAreaService areaService)
    {
        _dailyPricesService = dailyPricesService;
        _httpClient = new HttpClient();
        _areaService = areaService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine($"{DateTime.Now} - Getting tomorrows prices");
            Console.WriteLine(_requestUri);

            var date = DateTime.Now.AddDays(1).Date;

            var areas = await _areaService.GetAllAsync();
            var json = await GetDataFromApi();

            if (!areas.IsSuccessful)
                Console.WriteLine(areas.ErrorMessage);
            else
            {
                foreach (var area in areas.Data)
                {
                    Console.WriteLine($"Getting prices for {area.Name} : ");

                    List<HourlyPriceCreateDto> hourlyPricesForArea = ParseJsonData(json, area, date);

                    AddDataToDatabase(area, hourlyPricesForArea);


                }
            }
        }
    }

    private async Task AddDataToDatabase(AreaResponseDto area, List<HourlyPriceCreateDto> hourlyPrices)
    {
        if (hourlyPrices.Count > 0)
        {
            var addResult = await _dailyPricesService.AddAsync(area.Id, hourlyPrices[0].Date, hourlyPrices);
            if (addResult.IsSuccessful)
                Console.WriteLine($"    succesfully added prices for {area.Name} on {hourlyPrices[0].Date}");
            else
            {
                Console.WriteLine(
                    $"  failed to add prices for {area.Name} on {hourlyPrices[0].Date.Date} : {addResult.ErrorMessage}");
            }
        }
        else
        {
            Console.WriteLine($"    No data found for Area: {area.Name}");
        }

        var result = await _dailyPricesService.AddAsync(area.Id, hourlyPrices[0].Date.Date, hourlyPrices);
    }

    private List<HourlyPriceCreateDto> ParseJsonData(JsonElement json, AreaResponseDto area, DateTime date)
    {
        var result = new List<HourlyPriceCreateDto>();

        foreach (var row in json.GetProperty("data").GetProperty("Rows").EnumerateArray())
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(row.GetProperty("Name").GetString(),
                "^[0-2].*"))
            {
                foreach (var column in row.GetProperty("Columns").EnumerateArray())
                {
                    try
                    {
                        if (column.GetProperty("Name").GetString() == area.Name)
                        {
                            var periodString = System.Text.RegularExpressions.Regex
                                .Match(row.GetProperty("Name").GetString(), "([0-9]+)").Groups[1].Value;
                            var period = int.Parse(periodString);
                            var priceString = column.GetProperty("Value").GetString().Replace(" ", "");
                            var price = int.Parse(priceString.Split(',')[0]) / 10;

                            result.Add(new HourlyPriceCreateDto(period, price, date));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error getting data for {area.Name}: {ex.Message}");
                    }
                }
            }
        }

        return result;
    }

    private async Task<JsonElement> GetDataFromApi()
    {
        var response = await _httpClient.GetAsync(_requestUri);
        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content).RootElement;
    }
}