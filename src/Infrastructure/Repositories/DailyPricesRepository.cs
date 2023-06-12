using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DailyPricesRepository : IDailyPricesRepository
{
    private readonly EnergyPricesDbContext _context;

    public DailyPricesRepository(EnergyPricesDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Asynchronously retrieves all daily price collections for a specified area.
    /// </summary>
    /// <param name="areaId">The unique identifier of the area for which the daily price collections are being retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The value of the TResult parameter contains an
    /// enumerable list of the retrieved daily price collections, or an empty enumerable list if no daily price collections are found for the given area.</returns>
    public async Task<IEnumerable<DailyPriceCollection>> GetAllForAreaAsync(Guid areaId)
    {
        var dailyPrices = await _context.DailyPrices.ToListAsync();
        
        return dailyPrices ?? Enumerable.Empty<DailyPriceCollection>();
    }

    /// <summary>
    /// Asynchronously retrieves all daily price collections for a specified date.
    /// </summary>
    /// <param name="date">The date for which the daily price collections are being retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The value of the TResult parameter contains an
    /// enumerable list of the retrieved daily price collections or an empty enumerable list if no daily price collections are found for the given date.</returns>
    public async Task<IEnumerable<DailyPriceCollection>> GetAllForDateAsync(DateTime date)
    {
        var dailyPrices = await _context.DailyPrices
            .Where(d => d.Date.Date == date.Date)
            .ToListAsync();
        return dailyPrices ?? Enumerable.Empty<DailyPriceCollection>();
    }

    /// <summary>
    /// Asynchronously retrieves a daily price collection with a specified ID from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the daily price collection being retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The value of the TResult parameter is the retrieved
    /// daily price collection, or null if the daily price collection with the given ID was not found.</returns>
    public async Task<DailyPriceCollection?> GetByIdAsync(Guid id)
    {
        var dailyPrices = await _context.DailyPrices.FindAsync(id);
        if(dailyPrices == null)
        {
            Console.WriteLine($"Daily price collection with ID {id} was not found.");
            return null;
        }
        
        return dailyPrices;
    }

    /// <summary>
    /// Asynchronously retrieves a daily price collection for the specified area and date from the database.
    /// </summary>
    /// <param name="areaId">The unique identifier of the area for which the daily price collection is being retrieved.</param>
    /// <param name="date">The date for which the daily price collection is being retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The value of the TResult parameter is the retrieved
    /// daily price collection, or null if no daily price collection was found.</returns>
    public async Task<DailyPriceCollection?> GetByAreaAndDate(Guid areaId, DateTime date)
    {
        var dailyPrices = await _context.DailyPrices
            .Include(dpc => dpc.Prices)
            .FirstOrDefaultAsync(dpc => dpc.AreaId == areaId && dpc.Date == date);
        
        if(dailyPrices == null)
            return null;
        
        return dailyPrices;
    }

    /// <summary>
    /// Asynchronously adds a daily price collection for the specified area to the database.
    /// </summary>
    /// <param name="area">The area for which the daily price collection is being added.</param>
    /// <param name="dailyPriceCollection">The daily price collection to add.</param>
    /// <returns>The added daily price collection if successful, otherwise null.</returns>
    public async Task<DailyPriceCollection?> AddAsync(Area area, DailyPriceCollection dailyPriceCollection)
    {
        _context.DailyPrices.Add(dailyPriceCollection);
        var addedItems = await _context.SaveChangesAsync();
        if(addedItems > 0)
            return dailyPriceCollection;
        return null;
    }

    /// <summary>
    /// Asynchronously deletes the specified daily price collection from the database.
    /// </summary>
    /// <param name="dailyPriceCollection">The daily price collection to delete.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    public async Task<bool> DeleteAsync(DailyPriceCollection dailyPriceCollection)
    {
        _context.DailyPrices.Remove(dailyPriceCollection);
        
        var deletedRows = await _context.SaveChangesAsync();
        if (deletedRows > 0)
        {
            Console.WriteLine($"Succesfully deleted Daily price collection with ID: {dailyPriceCollection.Id}.");
            return true;
        }
        else
        {
            Console.WriteLine($"Failed to delete Daily price collection with ID: {dailyPriceCollection.Id}.");
            return false;
        }
    }
}