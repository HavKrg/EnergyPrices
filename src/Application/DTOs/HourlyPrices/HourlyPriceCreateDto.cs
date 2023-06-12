namespace Application.DTOs.HourlyPrices;

public class HourlyPriceCreateDto {
    public int Hour { get; set; }
    public int Price { get; set; }
    public DateTime Date { get; set; }

    public HourlyPriceCreateDto(int hour, int price, DateTime date)
    {
        
        Hour = hour;
        Price = price;
        Date = date;
    }
}