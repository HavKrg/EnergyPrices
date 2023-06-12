using Application.DTOs.DailyPriceCollection;
using Core.Entities;

namespace Application.Mappers.DTOs;

public class DailyPriceCollectionMapper {
    public static DailyPriceCollectionResponseDto DailyPriceCollectionToDailyPriceCollectionResponseDto(DailyPriceCollection dailyPriceCollection)
    {
        return new DailyPriceCollectionResponseDto(dailyPriceCollection.Id, dailyPriceCollection.AreaId, dailyPriceCollection.Date, dailyPriceCollection.Prices);
    }
}