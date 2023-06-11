using Application.DTOs.Area;
using Application.Interfaces;

namespace Application.Handlers.Areas;

public class CreateAreaHandler {
    
    private readonly IAreaService _areaService;

    public CreateAreaHandler(IAreaService areaService)
    {
        _areaService = areaService;
    }
    
    public async Task<AreaResponseDto> Handle(AreaCreateDto areaCreateDto)
    {
        return await _areaService.AddAsync(areaCreateDto);
    }
}