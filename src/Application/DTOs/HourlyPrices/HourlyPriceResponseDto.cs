namespace Application.DTOs.HourlyPrices;

public class HourlyPriceResponseDto {
    public int Hour { get; set; }
    public int Price { get; set; }
    public DateTime Date { get; set; }
    public Guid AreaId { get; set; }

    public HourlyPriceResponseDto(int hour, int price, DateTime date, Guid areaId)
    {
        Hour = hour;
        Price = price;
        Date = date;
        AreaId = areaId;
        
    }
}