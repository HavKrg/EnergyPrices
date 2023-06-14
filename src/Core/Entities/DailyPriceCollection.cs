
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class DailyPriceCollection : BaseEntity
{
    [Required]
    public Guid AreaId { get; set; }
    [Required]
    public DateTime Date { get; set; }
    public ICollection<HourlyPrice> Prices { get; set; } = new Collection<HourlyPrice>(); 

    public DailyPriceCollection(Guid areaId, DateTime date)
    {
            AreaId = areaId;
            Date = date;
            Created = DateTime.Now;
            Modified = DateTime.Now;
    }
}
