using Application.DTOs.DailyPriceCollection;
using Application.DTOs.HourlyPrices;
using Application.Interfaces;
using Application.Mappers.DTOs;
using Core.Entities;
using Core.Interfaces;

namespace Application.Services;



/// <summary>
/// DailyPricesService is a class that implements the IDailyPricesService interface, providing methods to interact with the daily prices data.
/// </summary>
public class DailyPricesService : IDailyPricesService {
    
    private readonly IDailyPricesRepository _dailyPricesRepository;
    private readonly IAreaRepository _areaRepository;

    public DailyPricesService(IDailyPricesRepository dailyPricesRepository, IAreaRepository areaRepository)
    {
        _dailyPricesRepository = dailyPricesRepository;
        _areaRepository = areaRepository;
    }

    ///<summary>
    /// Asynchronously retrieves a collection of DailyPriceCollectionResponseDto for the given area.
    /// </summary>
    /// <param name="areaId">The ID for which to retrieve the daily price collections.</param>
    /// <returns>A Task resulting in an OperationResult containing an IEnumerable of DailyPriceCollectionResponseDto.</returns>
    public async Task<OperationResult<IEnumerable<DailyPriceCollectionResponseDto>>> GetAllForAreaAsync(Guid areaId)
    {
       var dailyPriceCollections = (await _dailyPricesRepository.GetAllForAreaAsync(areaId)).ToList();
       
       if(!dailyPriceCollections.Any())
           return OperationResult<IEnumerable<DailyPriceCollectionResponseDto>>.Failure($"No prices for AreaId: {areaId} yet"); 
       
       var response = dailyPriceCollections.Select(DailyPriceCollectionMapper
           .DailyPriceCollectionToDailyPriceCollectionResponseDto);
       
       return OperationResult<IEnumerable<DailyPriceCollectionResponseDto>>
                .Success(response);
    }

    ///<summary>
    /// Asynchronously retrieves a collection of DailyPriceCollectionResponseDto for the given date.
    /// </summary>
    /// <param name="date">The date for which to retrieve the daily price collections.</param>
    /// <returns>A Task resulting in an OperationResult containing an IEnumerable of DailyPriceCollectionResponseDto.</returns>
    public async Task<OperationResult<IEnumerable<DailyPriceCollectionResponseDto>>> GetAllForDateAsync(DateTime date)
    {
        var dailyPriceCollections = (await _dailyPricesRepository.GetAllForDateAsync(date)).ToList();
        
        if(!dailyPriceCollections.Any())
            return OperationResult<IEnumerable<DailyPriceCollectionResponseDto>>.Failure($"No prices for Date: {date.Date} yet");
        
        var response = dailyPriceCollections.Select(DailyPriceCollectionMapper
            .DailyPriceCollectionToDailyPriceCollectionResponseDto);

        return OperationResult<IEnumerable<DailyPriceCollectionResponseDto>>
            .Success(response);
    }

    
    
    public async Task<OperationResult<DailyPriceCollectionResponseDto>> GetByAreaAndDate(Guid areaId, DateTime date)
    {
        var dailyPriceCollection = await _dailyPricesRepository.GetByAreaAndDate(areaId, date);
        
        if(dailyPriceCollection == null)
            return OperationResult<DailyPriceCollectionResponseDto>.Failure($"No prices for Area: {areaId} on {date}");
        
        return OperationResult<DailyPriceCollectionResponseDto>.Success(DailyPriceCollectionMapper.DailyPriceCollectionToDailyPriceCollectionResponseDto(dailyPriceCollection));
        
    }

    /// <summary>
    /// Retrieves a DailyPriceCollectionResponseDto by its ID (dailyPriceCollectionId) asynchronously.
    /// </summary>
    /// <param name="dailyPriceCollectionId">The unique identifier (GUID) of the daily price collection.</param>
    /// <returns>A Task resulting in an OperationResult containing a DailyPriceCollectionResponseDto if found, otherwise a failure message.</returns>
    public async Task<OperationResult<DailyPriceCollectionResponseDto>> GetByIdAsync(Guid dailyPriceCollectionId)
    {
        var dailyPriceCollection = await _dailyPricesRepository.GetByIdAsync(dailyPriceCollectionId);
        if(dailyPriceCollection == null)
            return OperationResult<DailyPriceCollectionResponseDto>
                    .Failure($"unable to find an Daily price collection with ID: {dailyPriceCollectionId}");
        
        return OperationResult<DailyPriceCollectionResponseDto>
                .Success(DailyPriceCollectionMapper.DailyPriceCollectionToDailyPriceCollectionResponseDto(dailyPriceCollection));
    }

    /// <summary>
    /// Asynchronously adds a new DailyPriceCollection to the specified area, with prices provided as an IEnumerable of HourlyPriceCreateDto objects.
    /// </summary>
    /// <param name="areaId">The unique identifier (GUID) of the area to add the daily price collection.</param>
    /// <param name="date">The date for the daily price collection being added.</param>
    /// <param name="prices">The collection of HourlyPriceCreateDto objects representing the hourly prices for the daily price collection.</param>
    /// <returns>A Task resulting in an OperationResult containing a DailyPriceCollectionResponseDto if the addition is successful, otherwise a failure message.</returns>
    public async Task<OperationResult<DailyPriceCollectionResponseDto>> AddAsync(Guid areaId, DateTime date, IEnumerable<HourlyPriceCreateDto> prices)
    {
        var area = await _areaRepository.GetByIdAsync(areaId);
        
        if(area == null)
            return OperationResult<DailyPriceCollectionResponseDto>
                    .Failure($"unable to find an Area with ID: {areaId}");

        var priceExists = await _dailyPricesRepository.GetByAreaAndDate(areaId, date);

        if(priceExists != null)
            return OperationResult<DailyPriceCollectionResponseDto>
                    .Failure($"database already have prices for {area.Name} on {date.Date}");

        var dailyPriceCollection = new DailyPriceCollection(areaId, date);

        foreach (var hourlyPrice in prices)
        {
            dailyPriceCollection.Prices.Add(HourlyPriceMappers.CreateHourlyPriceMapper(hourlyPrice, date, areaId));
        }
        
        
        var response = await _dailyPricesRepository.AddAsync(area, dailyPriceCollection);
        
        if(response == null)
            return OperationResult<DailyPriceCollectionResponseDto>.Failure($"unable to add Daily price collection to {area.Name}");
        return OperationResult<DailyPriceCollectionResponseDto>.Success(DailyPriceCollectionMapper.DailyPriceCollectionToDailyPriceCollectionResponseDto(dailyPriceCollection));
    }

    /// <summary>
    /// Asynchronously deletes a DailyPriceCollection by its ID (dailyPriceCollectionId).
    /// </summary>
    /// <param name="dailyPriceCollectionId">The unique identifier (GUID) of the daily price collection to be deleted.</param>
    /// <returns>A Task resulting in an OperationResult indicating success or failure of the deletion process.</returns>
    public async Task<OperationResult> DeleteAsync(Guid dailyPriceCollectionId)
    {
        var dailyPriceCollection = await _dailyPricesRepository.GetByIdAsync(dailyPriceCollectionId);
        
        if(dailyPriceCollection == null)
            return OperationResult.Failure($"unable to find an Daily price collection with ID: {dailyPriceCollectionId}");
        
        var response = await _dailyPricesRepository.DeleteAsync(dailyPriceCollection);
        if(response)
            return OperationResult.Success();
        return OperationResult.Failure($"unable to delete Daily price collection wit id: {dailyPriceCollectionId}");
    }
}