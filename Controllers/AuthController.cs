using LibraryManagementSystem.Data;
using LibraryManagementSystem.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public AuthController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Auth Controller is Working");
        }

       [HttpPost("register")]
public async Task<IActionResult> Register(RegisterUserRequest request)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    var membershipPlan = await _context.MembershipPlans
        .FirstOrDefaultAsync(x => x.MembershipPlanId == request.MembershipPlanId);

    if (membershipPlan == null)
    {
        return BadRequest("Invalid Membership Plan.");
    }

    return Ok("Membership Plan Found");
}
    }
}