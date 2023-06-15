using System.Text.Json;
using Application.DTOs.Area;
using Application.DTOs.HourlyPrices;
using Application.Interfaces;
using Quartz;

namespace WebApi.QuartzJobs.PriceGrabber;

public class PriceGrabberJob : IJob {
    
    private readonly IDailyPricesService _dailyPricesService;
    private readonly IAreaService _areaService;
    private readonly HttpClient _httpClient;
    private readonly string _requestUri =
        "https://www.nordpoolgroup.com/api/marketdata/page/10?currency=NOK,NOK,EUR,EUR";

    public PriceGrabberJob(IDailyPricesService dailyPricesService, IAreaService areaService)
    {
        _dailyPricesService = dailyPricesService;
        _areaService = areaService;
        _httpClient = new HttpClient();
    }

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"{DateTime.Now} - Running Pricegrabber job");
        var areas = await _areaService.GetAllAsync();
        var json = await GetDataFromApi();
        var date = DateTime.Today.AddDays(1).Date;
        
        if (!areas.IsSuccessful)
            Console.WriteLine(areas.ErrorMessage);
        else
        {
            if (areas.Data != null)
                foreach (var area in areas.Data)
                {
                    Console.WriteLine($"Getting prices for {area.Name} : ");
                    List<HourlyPriceCreateDto> hourlyPricesForArea = ParseJsonData(json, area, date);
                    await AddDataToDatabase(area, date, hourlyPricesForArea);
                }
        }
    }
    
    /// <summary>
    /// Adds hourly price data for a given area and date to the database.
    /// </summary>
    /// <param name="area">An AreaResponseDto object containing the details of a specific area.</param>
    /// <param name="date">The date for which the price data is being added.</param>
    /// <param name="hourlyPrices">A list of HourlyPriceCreateDto objects containing price data for each hour of the day.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task AddDataToDatabase(AreaResponseDto area, DateTime date, List<HourlyPriceCreateDto> hourlyPrices)
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

        var result = await _dailyPricesService.AddAsync(area.Id, date, hourlyPrices);
    }

    /// <summary>
    /// Parses JSON data from an API response to create a list of HourlyPriceCreateDto objects.
    /// </summary>
    /// <param name="json">The root JsonElement to parse for hourly price data.</param>
    /// <param name="area">An AreaResponseDto object containing the details of a specific area.</param>
    /// <param name="date">The date for which the price data is being parsed.</param>
    /// <returns>A list of HourlyPriceCreateDto objects created from the parsed JSON data.</returns>
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

    /// <summary>
    /// Retrieves JSON data from external API using httpclient.
    /// </summary>
    /// <returns>The root JsonElement from the returned API data.</returns>
    private async Task<JsonElement> GetDataFromApi()
    {
        var response = await _httpClient.GetAsync(_requestUri);
        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content).RootElement;
    }
}