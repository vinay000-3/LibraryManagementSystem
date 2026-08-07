using LibraryManagementSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("admin")]
        public async Task<IActionResult> GetAdminDashboard()
        {
            var dashboard = await _dashboardService.GetAdminDashboardAsync();

            return Ok(dashboard);
        }

        [HttpGet("user")]
[Authorize(Roles = "Member")]
public async Task<IActionResult> GetUserDashboard()
{
    try
    {
        var result = await _dashboardService.GetUserDashboardAsync();
        return Ok(result);
    }
    catch (Exception ex)
    {
        return BadRequest(new
        {
            Success = false,
            Message = ex.Message
        });
    }
}
    }
}