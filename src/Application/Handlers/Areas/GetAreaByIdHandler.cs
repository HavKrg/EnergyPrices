/*using Application.DTOs.Area;
using Application.Interfaces;

namespace Application.Handlers.Areas;

public class GetAreaByIdHandler {
    private readonly IAreaService _areaService;

    public GetAreaByIdHandler(IAreaService areaService)
    {
        _areaService = areaService;
    }
    
    public async Task<AreaResponseDto?> Handle(Guid id)
    {
        return await _areaService.GetByIdAsync(id);
    }
}*/