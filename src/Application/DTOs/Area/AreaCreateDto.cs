namespace Application.DTOs.Area;

public class AreaCreateDto {
    public string Name { get; set; }
    public string Description { get; set; }

    public AreaCreateDto(string name, string description)
    {
        Name = name;
        Description = description;
    }
}