using System.Text.Json;
using Application.DTOs.HourlyPrices;
using Application.Interfaces;

namespace WebApi.Services;

public class PriceGrabberService : BackgroundService
{
    private readonly IDailyPricesService _dailyPricesService;
    private readonly IAreaService _areaService;
    private readonly HttpClient _httpClient;
    int _counter;

    private readonly string _requestUri =
        "https://www.nordpoolgroup.com/api/marketdata/page/10?currency=NOK,NOK,EUR,EUR";

    public PriceGrabberService(IDailyPricesService dailyPricesService, IAreaService areaService)
    {
        _dailyPricesService = dailyPricesService;
        _httpClient = new HttpClient();
        _areaService = areaService;
        _counter = 0;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine($"{DateTime.Now} - Getting tomorrows prices");
            /*Console.WriteLine(_requestUri);
        var areas = await _areaService.GetAllAsync();
        var response = await _httpClient.GetAsync(_requestUri);
        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content).RootElement;
        var date = DateTime.Now.AddDays(1).Date;

        if (!areas.IsSuccessful)
            Console.WriteLine("No areas in DB yet");
        else
        {
            foreach (var area in areas.Data)
            {
                Console.WriteLine($"Getting prices for {area.Name}");
                var result = new List<HourlyPriceCreateDto>();

                foreach (var row in json.GetProperty("data").GetProperty("Rows").EnumerateArray())
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(row.GetProperty("Name").GetString(), "^[0-2].*"))
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

                if (result.Count > 0)
                {
                    var addResult = await _dailyPricesService.AddAsync(area.Id, result[0].Date, result);
                    if (addResult.IsSuccessful)
                        Console.WriteLine($"succesfully added prices for {area.Name} on {date}");
                    else
                    {
                        Console.WriteLine($"failed to add prices for {area.Name} on {date} : {addResult.ErrorMessage}");

                    }

                }
                else
                {
                    Console.WriteLine($"{DateTime.Now} No data found for Area: {area.Name}");
                }
            }
        }*/
            Console.WriteLine($"{DateTime.Now} - Run number {_counter}");
            _counter +=1;
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);    
        }
    }
}