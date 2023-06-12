using System.Collections.ObjectModel;

namespace Core.Entities;

public class Area : BaseEntity
{
    public string Name { get; set; }
    public string NormalizedName { get; set; }
    public string? Description { get; set; }
    public ICollection<DailyPriceCollection> DailyPrices { get; set; } = new Collection<DailyPriceCollection>();
    
    public Area(string name, string description = "No description yet")
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name cannot be null or empty.");

        Name = name;
        Description = description;
        NormalizedName = name.ToUpperInvariant();
    }
}