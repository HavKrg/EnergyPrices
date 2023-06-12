using Core.Entities;

namespace Core.Interfaces;

/// <summary>
/// IAreaRepository is an interface representing a repository for managing Area data.
/// </summary>
/// <remarks>
/// Provides methods for retrieving, adding, and deleting Areas.
/// </remarks>
public interface IAreaRepository
{
    Task<IEnumerable<Area>> GetAllAsync();
    Task<Area?> GetByIdAsync(Guid areaId);
    Task<Area?> GetByNameAsync(string name);
    Task<Area?> AddAsync(Area area);
    Task<Area?> UpdateAsync(Area area);
    Task<bool> DeleteAsync(Area area);
    
}