using System.Runtime.Versioning;
using Application.DTOs.Area;
using Application.Interfaces;
using Application.Mappers.DTOs;
using Application.Services;
using Core.Entities;
using Core.Interfaces;

namespace Application.Services;

public class AreaService : IAreaService
{
    
    private readonly IAreaRepository _areaRepository;

    public AreaService(IAreaRepository areaRepository)
    {
        _areaRepository = areaRepository;
    }


    public async Task<OperationResult<IEnumerable<AreaResponseDto>>> GetAllAsync()
    {
        var areas = await _areaRepository.GetAllAsync();
        
        var response = areas.Select(area => AreaMappers.AreaToAreaResponseMapper(area)).ToList();
        
        return OperationResult<IEnumerable<AreaResponseDto>>
                .Success(response);
    }

    public async Task<OperationResult<AreaResponseDto>> GetByIdAsync(Guid areaId)
    {
        var area = await _areaRepository.GetByIdAsync(areaId);
        
        if(area == null)
            return OperationResult<AreaResponseDto>
                    .Failure($"unable to find an Area with ID: {areaId}");

        return OperationResult<AreaResponseDto>
                .Success(AreaMappers.AreaToAreaResponseMapper(area));
    }

    public async Task<OperationResult<AreaResponseDto>> GetByNameAsync(string name)
    {
        var area  = await _areaRepository.GetByNameAsync(name);
        
        if(area == null)
            return OperationResult<AreaResponseDto>
                    .Failure($"unable to find an Area with Name: {name} from database");
        
        return OperationResult<AreaResponseDto>
                .Success(AreaMappers.AreaToAreaResponseMapper(area));
    }

    public async Task<OperationResult<AreaResponseDto>> AddAsync(AreaCreateDto areaCreateDto)
    {
        var newArea = AreaMappers.CreateAreaToAreaMapper(areaCreateDto);
                
        newArea = await _areaRepository.AddAsync(newArea);

        if(newArea == null)
            return OperationResult<AreaResponseDto>
                    .Failure($"unable to add {areaCreateDto.Name} to database");
        return OperationResult<AreaResponseDto>.Success(AreaMappers.AreaToAreaResponseMapper(newArea));
    }

    public async Task<OperationResult<AreaResponseDto>> UpdateAsync(Guid areaId, AreaUpdateDto areaUpdateDto)
    {
        var existingArea = await _areaRepository.GetByIdAsync(areaId);
        if(existingArea == null)
            return OperationResult<AreaResponseDto>.Failure($"unable to find an Area with ID: {areaId}");
        
        existingArea.Name = areaUpdateDto.Name;
        existingArea.NormalizedName = areaUpdateDto.Name.ToUpperInvariant();
        existingArea.Description = areaUpdateDto.Description;
        existingArea.Modified = DateTime.Now;
        var response = await _areaRepository.UpdateAsync(existingArea);
        if(response == null)
            return OperationResult<AreaResponseDto>.Failure($"unable to delete the Area with ID: {areaId}");
        return OperationResult<AreaResponseDto>.Success(AreaMappers.AreaToAreaResponseMapper(existingArea));
    }

    public async Task<OperationResult> DeleteAsync(Guid areaId)
    {
        var area = await _areaRepository.GetByIdAsync(areaId);
        if(area == null)
            return OperationResult.Failure($"unable to find an Area with ID: {areaId}");

        var result = await _areaRepository.DeleteAsync(area);
        if(result)
            return OperationResult.Success();
        return OperationResult.Failure($"unable to delete Area with Id {areaId}");
    }
}