using System.Runtime.Versioning;
using Application.DTOs.Area;
using Application.Interfaces;
using Application.Mappers.DTOs;
using Application.Services;
using Core.Entities;
using Core.Interfaces;

namespace Application.Services;


/// <summary>
/// AreaService is a class that implements IAreaService which provides a set of operations to interact with areas using IAreaRepository.
/// </summary>
public class AreaService : IAreaService
{
    private readonly IAreaRepository _areaRepository;

    public AreaService(IAreaRepository areaRepository)
    {
        _areaRepository = areaRepository;
    }


    /// <summary>
    /// Asynchronously retrieves all areas stored in the repository and returns an OperationResult containing a collection of AreaResponseDto objects or a Failure message.
    /// </summary>
    /// <returns>
    /// A Task<OperationResult<IEnumerable<AreaResponseDto>>> object containing the success or failure information of the operation along with
    /// the collection of AreaResponseDto objects.
    /// </returns>
    public async Task<OperationResult<IEnumerable<AreaResponseDto>>> GetAllAsync()
    {
        var areas = (await _areaRepository.GetAllAsync()).ToList();

        if (!areas.Any())
            return OperationResult<IEnumerable<AreaResponseDto>>.Failure("No areas added yet");

        var response = areas.Select(AreaMappers
            .AreaToAreaResponseMapper);

        return OperationResult<IEnumerable<AreaResponseDto>>
            .Success(response);
    }


    /// <summary>
    /// Asynchronously retrieves an area by its unique identifier and returns an OperationResult containing the AreaResponseDto or a Failure message.
    /// </summary>
    /// <param name="areaId">The unique identifier of the area to fetch.</param>
    /// <returns>
    /// A Task<OperationResult<AreaResponseDto>> object containing the success or failure information of the operation along with the AreaResponseDto.
    /// </returns>
    public async Task<OperationResult<AreaResponseDto>> GetByIdAsync(Guid areaId)
    {
        var area = await _areaRepository.GetByIdAsync(areaId);

        if (area == null)
            return OperationResult<AreaResponseDto>
                .Failure($"unable to find an Area with ID: {areaId}");

        return OperationResult<AreaResponseDto>
            .Success(AreaMappers.AreaToAreaResponseMapper(area));
    }


    /// <summary>
    /// Asynchronously finds an area by its name and returns an OperationResult containing the AreaResponseDto or a Failure message.
    /// </summary>
    /// <param name="name">The name of the area to search.</param>
    /// <returns>
    /// A Task<OperationResult<AreaResponseDto>> object containing the success or failure information of the operation along with the AreaResponseDto.
    /// </returns>
    public async Task<OperationResult<AreaResponseDto>> GetByNameAsync(string name)
    {
        var area = await _areaRepository.GetByNameAsync(name);

        if (area == null)
            return OperationResult<AreaResponseDto>
                .Failure($"unable to find an Area with Name: {name} from database");

        return OperationResult<AreaResponseDto>
            .Success(AreaMappers.AreaToAreaResponseMapper(area));
    }

    /// <summary>
    /// Asynchronously adds a new area to the database using the provided AreaCreateDto object.
    /// </summary>
    /// <param name="areaCreateDto">AreaCreateDto object containing the information needed to create a new area.</param>
    /// <returns>
    /// A Task<OperationResult<AreaResponseDto>> object containing the success or failure information of the operation along with the created area.
    /// </returns>
    public async Task<OperationResult<AreaResponseDto>> AddAsync(AreaCreateDto areaCreateDto)
    {
        var newArea = AreaMappers.CreateAreaToAreaMapper(areaCreateDto);

        newArea = await _areaRepository.AddAsync(newArea);

        if (newArea == null)
            return OperationResult<AreaResponseDto>
                .Failure($"unable to add {areaCreateDto.Name} to database");
        return OperationResult<AreaResponseDto>.Success(AreaMappers.AreaToAreaResponseMapper(newArea));
    }


    /// <summary>
    /// Updates an existing area with the provided AreaUpdateDto
    /// </summary>
    /// <param name="areaId">Guid of the area to be updated</param>
    /// <param name="areaUpdateDto">AreaUpdateDto object containing the updated information of the area</param>
    /// <returns>
    /// A Task<OperationResult<AreaResponseDto>> object containing the success or failure information of the operation along with the updated area
    /// </returns>
    public async Task<OperationResult<AreaResponseDto>> UpdateAsync(Guid areaId, AreaUpdateDto areaUpdateDto)
    {
        var existingArea = await _areaRepository.GetByIdAsync(areaId);
        if (existingArea == null)
            return OperationResult<AreaResponseDto>.Failure($"unable to find an Area with ID: {areaId}");

        existingArea.Name = areaUpdateDto.Name;
        existingArea.NormalizedName = areaUpdateDto.Name.ToUpperInvariant();
        existingArea.Description = areaUpdateDto.Description;
        existingArea.Modified = DateTime.Now;
        var response = await _areaRepository.UpdateAsync(existingArea);
        if (response == null)
            return OperationResult<AreaResponseDto>.Failure($"unable to delete the Area with ID: {areaId}");
        return OperationResult<AreaResponseDto>.Success(AreaMappers.AreaToAreaResponseMapper(existingArea));
    }


    /// <summary>
    /// Asynchronously deletes an area by its unique identifier.
    /// </summary>
    /// <param name="areaId">The unique identifier of the area to be deleted.</param>
    /// <returns>
    /// An OperationResult object indicating the success or failure of the operation.
    /// </returns>
    public async Task<OperationResult> DeleteAsync(Guid areaId)
    {
        var area = await _areaRepository.GetByIdAsync(areaId);
        if (area == null)
            return OperationResult.Failure($"unable to find an Area with ID: {areaId}");

        var response = await _areaRepository.DeleteAsync(area);
        if (response)
            return OperationResult.Success();
        return OperationResult.Failure($"unable to delete Area with Id {areaId}");
    }
}