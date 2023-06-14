
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace Core.Entities;


public class HourlyPrice : BaseEntity
{
    [Required]
    public Guid DailyPriceCollectionId { get; set; }
    [Required]
    public Guid AreaId { get; set; }
    [Required]
    [Range(0,23)]
    public int Hour { get; set; }
    [Required]
    public int Price { get; set; }
    [Required]
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
