using Application.DTOs.Area;
using Application.Handlers.Areas;

namespace Application.Interfaces;

public interface IAreaService {
    Task<IEnumerable<AreaResponseDto>> GetAllAsync();
    Task<AreaResponseDto?> GetByIdAsync(Guid id);
    Task<AreaResponseDto?> GetByNameAsync(string name);
    Task<AreaResponseDto> AddAsync(AreaCreateDto areaCreateDto);
    Task<AreaResponseDto> UpdateAsync(Guid id, AreaUpdateDto areaUpdateDto);
    Task<bool> DeleteAsync(Guid id);
}

