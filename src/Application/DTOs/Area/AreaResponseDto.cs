namespace Application.DTOs.Area;

public class AreaResponseDto {
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public string Name { get; set; } = String.Empty;
    public string? Description { get; set; }
}