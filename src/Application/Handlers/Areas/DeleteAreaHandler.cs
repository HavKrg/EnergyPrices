using Application.Interfaces;

namespace Application.Handlers.Areas;

public class DeleteAreaHandler
{
    private readonly IAreaService _areaService;

    public DeleteAreaHandler(IAreaService areaService)
    {
        _areaService = areaService;
    }

    public async Task<bool> Handle(Guid id)
    {
        return await _areaService.DeleteAsync(id);
    }
}