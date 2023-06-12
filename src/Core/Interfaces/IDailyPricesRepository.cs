using Core.Entities;

namespace Core.Interfaces;

/// <summary>
/// IDailyPricesRepository is an interface representing a repository for managing daily price collection data.
/// </summary>
/// <remarks>
/// Provides methods for retrieving, adding, and deleting daily price collection data related to a specific area or date.
/// </remarks>
public interface IDailyPricesRepository {
    Task<IEnumerable<DailyPriceCollection>> GetAllForAreaAsync(Guid areaId);
    Task<IEnumerable<DailyPriceCollection>> GetAllForDateAsync(DateTime date);
    Task<DailyPriceCollection?> GetByIdAsync(Guid id);
    Task<DailyPriceCollection?> GetByAreaAndDate(Guid areaId, DateTime date);
    Task<DailyPriceCollection?> AddAsync(Area area, DailyPriceCollection dailyPriceCollection);
    /*Task<DailyPriceCollection?> UpdateAsync(Area area, DailyPriceCollection dailyPriceCollection);*/
    Task<bool> DeleteAsync(DailyPriceCollection dailyPriceCollection);
}
