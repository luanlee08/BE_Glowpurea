using BE_Glowpurea.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_Glowpurea.Controllers.Admin;

[Authorize(Roles = "admin")]
[ApiController]
[Route("api/admin/dashboard")]
public class AdminDashboardController : ControllerBase
{
    private readonly IDashboardService _service;

    public AdminDashboardController(IDashboardService service)
    {
        _service = service;
    }

    [HttpGet("overview")]
    public async Task<IActionResult> GetOverview()
    {
        return Ok(await _service.GetOverviewAsync());
    }
}
