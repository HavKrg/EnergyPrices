using System.Text.RegularExpressions;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AreaRepository : IAreaRepository
{
    private readonly EnergyPricesDbContext _context;

    public AreaRepository(EnergyPricesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Area>> GetAllAsync()
    {
        var areas = await _context.Areas.ToListAsync();
        
        return areas ?? Enumerable.Empty<Area>();
        
    }

    public async Task<Area?> GetByIdAsync(Guid id)
    {
        var area = await _context.Areas.FindAsync(id);
        if (area == null)
        {
            Console.WriteLine($"Area with ID {id} was not found.");
            return null;
        }
        
        return area;
    }

    public async Task<Area?> GetByNameAsync(string name)
    {
        var area = await _context.Areas.FirstOrDefaultAsync(a => a.NormalizedName == name.ToUpperInvariant());
        
        
        if (area == null)
        {
            Console.WriteLine($"Area with name {name} was not found.");
            return null;
        }

        return area;
    }

    public async Task<Area> AddAsync(Area area)
    {
        await _context.AddAsync(area);
        await _context.SaveChangesAsync();
        return area;
    }

    public async Task<Area> UpdateAsync(Area area)
    {
        _context.Areas.Update(area);
        await _context.SaveChangesAsync();
        return area;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var area = await GetByIdAsync(id);
        
        if(area == null)
            return false;

        _context.Areas.Remove(area);
        var deletedRows = await _context.SaveChangesAsync();
        
        if (deletedRows > 0)
        {
            Console.WriteLine($"Area with ID {id} successfully deleted.");
            return true;
        }
        else
        {
            Console.WriteLine($"No Area with ID {id} was found to delete.");
            return false;
        }
    }
}