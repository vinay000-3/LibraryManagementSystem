using LibraryManagementSystem.DTOs.UserResponseDto;
using LibraryManagementSystem.Enums;
using LibraryManagementSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] RegistrationStatus? status)
        {
            var result = await _userService.GetUsersAsync(status);
            return Ok(result);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var result = await _userService.GetUserByIdAsync(userId);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers(string keyword)
        {
            var result = await _userService.SearchUsersAsync(keyword);
            return Ok(result);
        }
    }
}