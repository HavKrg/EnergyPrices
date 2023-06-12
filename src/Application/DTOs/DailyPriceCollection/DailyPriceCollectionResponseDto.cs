using System.Collections.ObjectModel;
using Application.DTOs.Area;
using Application.DTOs.HourlyPrices;
using Core.Entities;

namespace Application.DTOs.DailyPriceCollection;

public class DailyPriceCollectionResponseDto {
    public Guid Id { get; set; }
    public Guid AreaId { get; set; }
    public DateTime Date { get; set; }
    public ICollection<HourlyPriceResponseDto> Prices { get; set; } = new Collection<HourlyPriceResponseDto>();

    public DailyPriceCollectionResponseDto(Guid id, Guid areaId, DateTime date, ICollection<HourlyPrice> prices)
    {
        Id = id;
        AreaId = areaId;
        Date = date;
        foreach (var hourlyPrice in prices)
        {
            Prices.Add(Mappers.DTOs.HourlyPriceMappers.HourlyPriceToHourlyPriceResponseDto(hourlyPrice));
        }
    }
}