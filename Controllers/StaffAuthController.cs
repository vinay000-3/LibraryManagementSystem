using BCrypt.Net;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.DTOs.Staff;
using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Enums;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffAuthController : ControllerBase
    {
        private readonly LibraryDbContext _context;
        private readonly StaffJwtHelper _staffJwtHelper;

        public StaffAuthController(
            LibraryDbContext context,
            StaffJwtHelper staffJwtHelper)
        {
            _context = context;
            _staffJwtHelper = staffJwtHelper;
        
        
    }

        [HttpPost("Login")]
public async Task<IActionResult> Login(StaffLoginRequestDto request)
{
    var employee = await _context.LibraryEmployees
        .FirstOrDefaultAsync(x => x.Email == request.Email);

    if (employee == null)
        return BadRequest("Invalid email or password.");

    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
        request.Password,
        employee.PasswordHash);

    if (!isPasswordValid)
        return BadRequest("Invalid email or password.");

    if (employee.Status != Enums.EmployeeStatus.Active)
        return BadRequest("Employee account is inactive.");

    string token = _staffJwtHelper.GenerateToken(employee);

    return Ok(new StaffLoginResponseDto
    {
        EmployeeId = employee.EmployeeId,
        FullName = employee.FullName,
        Role = employee.Role.ToString(),
        Token = token
    });
    
}

private async Task<IActionResult> RegisterEmployee(
    string fullName,
    string email,
    string password,
    StaffRole role)
{
    var existingEmployee = await _context.LibraryEmployees
        .FirstOrDefaultAsync(x => x.Email == email);

    if (existingEmployee != null)
        return BadRequest("Email already exists.");

    var lastEmployee = await _context.LibraryEmployees
        .OrderByDescending(x => x.EmployeeId)
        .FirstOrDefaultAsync();

    string employeeId = IdGenerator.GenerateEmployeeId(lastEmployee?.EmployeeId);

    var employee = new LibraryEmployee
    {
        EmployeeId = employeeId,
        FullName = fullName,
        Email = email,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
        Role = role,
        Status = EmployeeStatus.Active,
        CreatedDate = DateTime.Now
    };

    _context.LibraryEmployees.Add(employee);

    await _context.SaveChangesAsync();

    return Ok(new StaffRegisterResponseDto
    {
        EmployeeId = employee.EmployeeId,
        FullName = employee.FullName,
        Email = employee.Email,
        Role = employee.Role.ToString(),
        Message = $"{role} registered successfully."
    });
}
[Authorize(Roles = "Admin")]
[HttpPost("RegisterLibrarian")]
public async Task<IActionResult> RegisterLibrarian(RegisterLibrarianRequestDto request)
{
    return await RegisterEmployee(
        request.FullName,
        request.Email,
        request.Password,
        StaffRole.Librarian);
}

[Authorize(Roles = "Admin")]
[HttpPost("RegisterReturnVerificationOfficer")]
public async Task<IActionResult> RegisterReturnVerificationOfficer(RegisterReturnVerificationOfficerRequestDto request)
{
    return await RegisterEmployee(
        request.FullName,
        request.Email,
        request.Password,
        StaffRole.ReturnVerificationOfficer);
}
    }
}