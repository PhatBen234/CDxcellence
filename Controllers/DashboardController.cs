using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Unilever.CDExcellent.API.Services.IService;
using Unilever.CDExcellent.API.Models.Dto;

[Route("api/dashboard")]
[ApiController]
[Authorize] 
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetDashboardData(int userId)
    {
        var dashboardData = await _dashboardService.GetDashboardDataAsync(userId);

        if (dashboardData == null)
        {
            return NotFound(new { message = "User not found or no data available" });
        }

        return Ok(dashboardData);
    }
}
