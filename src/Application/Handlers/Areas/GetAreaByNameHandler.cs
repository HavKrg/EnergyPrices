/*using Application.DTOs.Area;
using Application.Interfaces;

namespace Application.Handlers.Areas;

public class GetAreaByNameHandler {
    private readonly IAreaService _areaService;

    public GetAreaByNameHandler(IAreaService areaService)
    {
        _areaService = areaService;
    }
    
    public async Task<AreaResponseDto?> Handle(string name)
    {
        return await _areaService.GetByNameAsync(name);
    }
}*/