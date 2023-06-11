using Core.Entities;

namespace Core.Interfaces;

public interface IAreaRepository
{
    Task<IEnumerable<Area>> GetAllAsync();
    Task<Area?> GetByIdAsync(Guid id);
    Task<Area?> GetByNameAsync(string name);
    Task<Area> AddAsync(Area area);
    Task<Area> UpdateAsync(Area area);
    Task<bool> DeleteAsync(Guid id);
    
}