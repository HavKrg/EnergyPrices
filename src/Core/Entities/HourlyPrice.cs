
using System.Security.AccessControl;

namespace Core.Entities;


public class HourlyPrice : BaseEntity
{
    public Guid DailyPriceCollectionId { get; set; }
    public Guid AreaId { get; set; }
    public int Hour { get; set; }
    public int Price { get; set; }
    public DateTime Date { get; set; }

    public HourlyPrice(int hour, int price, DateTime date, Guid areaId)
    {
        Hour = hour;
        Price = price;
        Date = date;
        Created = DateTime.Now;
        Modified = DateTime.Now;
        AreaId = areaId;
    }
}
