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

    public async Task<Area?> GetByIdAsync(Guid areaId)
    {
        var area = await _context.Areas.FindAsync(areaId);
        if (area == null)
        {
            Console.WriteLine($"Area with ID {areaId} was not found.");
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

    public async Task<Area?> AddAsync(Area area)
    {
        await _context.AddAsync(area);
        int addedRows = await _context.SaveChangesAsync();
        if(addedRows > 0)
            return area;
        return null;
    }

    public async Task<Area?> UpdateAsync(Area area)
    {
        _context.Areas.Update(area);
        int updatedRows = await _context.SaveChangesAsync();
        if(updatedRows > 0)
            return area;
        return null;
    }

    public async Task<bool> DeleteAsync(Area area)
    {
        _context.Areas.Remove(area);
        var deletedRows = await _context.SaveChangesAsync();
        
        if (deletedRows > 0)
        {
            Console.WriteLine($"Succesfully deleted Area with ID: {area.Id}.");
            return true;
        }
        else
        {
            Console.WriteLine($"Failed to delete Area with ID: {area.Id}.");
            return false;
        }
    }
}