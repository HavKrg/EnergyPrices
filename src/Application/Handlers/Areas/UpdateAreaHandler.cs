/*using Application.DTOs.Area;
using Application.Interfaces;

namespace Application.Handlers.Areas;

public class UpdateAreaHandler {
    private readonly IAreaService _areaService;
rets set "ConnectionStrings:DefaultConnection"  "Server=localhost,1433;Database=TestDb;User Id=sa;Password=yourStrong(!)Password;Trust Server Certificate=True;"
    public UpdateAreaHandler(IAreaService areaService)
    {
        _areaService = areaService;
    }
    public async Task<AreaResponseDto?> Handle(AreaUpdateDto areaUpdateDto)
    {
        return await _areaService.UpdateAsync(areaUpdateDto);
    }
}
*/