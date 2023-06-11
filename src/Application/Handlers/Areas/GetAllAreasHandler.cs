using Application.DTOs.Area;
using Application.Interfaces;

namespace Application.Handlers.Areas;

public class GetAllAreasHandler {
    private readonly IAreaService _areaService;

    public GetAllAreasHandler(IAreaService areaService)
    {
        _areaService = areaService;
    }
    
    public async Task<IEnumerable<AreaResponseDto>> Handle()
    {
        return await _areaService.GetAllAsync();
    }
}