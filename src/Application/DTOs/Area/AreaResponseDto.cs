namespace Application.DTOs.Area;

public class AreaResponseDto {
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public AreaResponseDto(Guid id, DateTime created, DateTime modified, string name, string? description)
    {
        Id = id;
        Created = created;
        Modified = modified;
        Name = name;
        Description = description;
    }
}