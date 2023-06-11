using Application.DTOs.Area;
using Core.Entities;

namespace Application.Interfaces;

public interface IAreaService {
    Task<OperationResult<IEnumerable<AreaResponseDto>>> GetAllAsync();
    Task<OperationResult<AreaResponseDto>> GetByIdAsync(Guid areaId);
    Task<OperationResult<AreaResponseDto>> GetByNameAsync(string name);
    Task<OperationResult<AreaResponseDto>> AddAsync(AreaCreateDto areaCreateDto);
    Task<OperationResult<AreaResponseDto>> UpdateAsync(Guid id, AreaUpdateDto areaUpdateDto);
    Task<OperationResult> DeleteAsync(Guid areaId);
}

