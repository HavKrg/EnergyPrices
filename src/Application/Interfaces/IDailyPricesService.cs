using Application.DTOs.DailyPriceCollection;
using Application.DTOs.HourlyPrices;
using Core.Entities;

namespace Application.Interfaces;

public interface IDailyPricesService {
    Task<OperationResult<IEnumerable<DailyPriceCollectionResponseDto>>> GetAllForAreaAsync(Guid areaId);
    Task<OperationResult<IEnumerable<DailyPriceCollectionResponseDto>>> GetAllForDateAsync(DateTime date);
    Task<OperationResult<DailyPriceCollectionResponseDto>> GetByAreaAndDate(Guid areaId, DateTime date);
    Task<OperationResult<DailyPriceCollectionResponseDto>> GetByIdAsync(Guid dailyPriceCollectionId);
    Task<OperationResult<DailyPriceCollectionResponseDto>> AddAsync(Guid areaId, DateTime date, IEnumerable<HourlyPriceCreateDto> prices);
    /*Task<OperationResult<DailyPriceCollectionResponseDto>> UpdateAsync(Guid id, AreaUpdateDto areaUpdateDto);*/
    Task<OperationResult> DeleteAsync(Guid dailyPriceCollectionId);
}