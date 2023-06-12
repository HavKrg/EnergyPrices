using Application.DTOs.HourlyPrices;
using Core.Entities;

namespace Application.Mappers.DTOs;

public class HourlyPriceMappers {
    public static HourlyPrice CreateHourlyPriceMapper(HourlyPriceCreateDto hourlyPriceCreateDto, DateTime date, Guid areaId)
    {
        return new HourlyPrice(hourlyPriceCreateDto.Hour,
            hourlyPriceCreateDto.Price, date, areaId);
    }
    
    public static HourlyPriceResponseDto HourlyPriceToHourlyPriceResponseDto(HourlyPrice hourlyPrice)
    {
        return new HourlyPriceResponseDto(hourlyPrice.Hour, hourlyPrice.Price, hourlyPrice.Date, hourlyPrice.AreaId);
    }
}