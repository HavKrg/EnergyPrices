using System.Runtime.Versioning;
using Application.DTOs.Area;
using Application.Interfaces;
using Application.Mappers.DTOs;
using Application.Services;
using Core.Interfaces;

namespace Application.Services;

public class AreaService : IAreaService
{
    
    private readonly IAreaRepository _areaRepository;

    public AreaService(IAreaRepository areaRepository)
    {
        _areaRepository = areaRepository;
    }


    public async Task<IEnumerable<AreaResponseDto>> GetAllAsync()
    {
        var areas = await _areaRepository.GetAllAsync();
        
        var response = areas.Select(area => AreaMappers.AreaToAreaResponseMapper(area)).ToList();
        
        return response;
    }

    public async Task<AreaResponseDto?> GetByIdAsync(Guid id)
    {
        var area = await _areaRepository.GetByIdAsync(id);
        
        if(area == null)
            return null;
        
        return AreaMappers.AreaToAreaResponseMapper(area);
    }

    public async Task<AreaResponseDto?> GetByNameAsync(string name)
    {
        var area  = await _areaRepository.GetByNameAsync(name);
        
        if(area == null)
            return null;
        
        return AreaMappers.AreaToAreaResponseMapper(area);
    }

    public async Task<AreaResponseDto> AddAsync(AreaCreateDto areaCreateDto)
    {
        var newArea = AreaMappers.CreateAreaToAreaMapper(areaCreateDto);
                
        newArea = await _areaRepository.AddAsync(newArea);
        return AreaMappers.AreaToAreaResponseMapper(newArea);
    }

    public async Task<AreaResponseDto> UpdateAsync(Guid areaId, AreaUpdateDto areaUpdateDto)
    {
        var existingArea = await _areaRepository.GetByIdAsync(areaId);
        
        existingArea.Name = areaUpdateDto.Name;
        existingArea.NormalizedName = areaUpdateDto.Name.ToUpperInvariant();
        existingArea.Description = areaUpdateDto.Description;
        existingArea.Modified = DateTime.Now;
        var response = await _areaRepository.UpdateAsync(existingArea);
        return AreaMappers.AreaToAreaResponseMapper(response);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        return _areaRepository.DeleteAsync(id);
    }
}