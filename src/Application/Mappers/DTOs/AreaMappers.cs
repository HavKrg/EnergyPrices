using Application.DTOs.Area;
using Core.Entities;

namespace Application.Mappers.DTOs;

public class AreaMappers
{
    public static Area CreateAreaToAreaMapper(AreaCreateDto areaCreateDto)
    {
        return new Area(areaCreateDto.Name, areaCreateDto.Description);
    }
    
    public static Area UpdateAreaToAreaMapper(AreaUpdateDto areaUpdateDto)
    {
        return new Area(areaUpdateDto.Name, areaUpdateDto.Description);
    }

    public static AreaResponseDto AreaToAreaResponseMapper(Area area)
    {
        return new AreaResponseDto()
        {
            Id = area.Id,
            Name = area.Name,
            Description = area.Description,
            Created = area.Created,
            Modified = area.Modified
        };
    }
}