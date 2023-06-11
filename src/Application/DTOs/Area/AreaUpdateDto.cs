namespace Application.DTOs.Area;

public class AreaUpdateDto {
    public string Name { get; set; }
    public string Description { get; set; }

    public AreaUpdateDto(string name, string description)
    {
        Name = name;
        Description = description;
    }
}