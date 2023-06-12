
using System.Collections.ObjectModel;

namespace Core.Entities;

public class DailyPriceCollection : BaseEntity
{
    public Guid AreaId { get; set; }
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
