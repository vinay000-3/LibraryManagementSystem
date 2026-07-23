using LibraryManagementSystem.Data;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Enums;
using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly LibraryDbContext _context;
private readonly JwtHelper _jwtHelper;

public AuthController(
    LibraryDbContext context,
    JwtHelper jwtHelper)
{
    _context = context;
    _jwtHelper = jwtHelper;
}
[AllowAnonymous]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Auth Controller is Working");
        }
        

        // ---------------- SELF REGISTRATION ----------------
[AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await CreateUser(
                request.FullName,
                request.Email,
                request.MobileNumber,
                request.Password,
                request.Address,
                request.DateOfBirth,
                request.Gender,
                request.MembershipPlanId,
                request.PaidMembershipFee,
                RegistrationStatus.Pending);
        }

        // ---------------- ADMIN REGISTER USERS----------------
[Authorize(Roles = "Admin")]
        [HttpPost("admin/register-member")]
        public async Task<IActionResult> AdminRegister(AdminRegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await CreateUser(
                request.FullName,
                request.Email,
                request.MobileNumber,
                request.Password,
                request.Address,
                request.DateOfBirth,
                request.Gender,
                request.MembershipPlanId,
                request.PaidMembershipFee,
                RegistrationStatus.Approved);
        }

        // ---------------- COMMON METHOD ----------------

        private async Task<IActionResult> CreateUser(
            string fullName,
            string email,
            string mobileNumber,
            string password,
            string address,
            DateTime dateOfBirth,
            Gender gender,
            string membershipPlanId,
            decimal paidMembershipFee,
            RegistrationStatus registrationStatus)
        {
            // Trim Inputs
            fullName = fullName.Trim();
            email = email.Trim().ToLower();
            mobileNumber = mobileNumber.Trim();
            address = address.Trim();

            // Validate Membership Plan
            var membershipPlan = await _context.MembershipPlans
                .FirstOrDefaultAsync(x => x.MembershipPlanId == membershipPlanId);

            if (membershipPlan == null)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Invalid Membership Plan Id."
                });
            }

            // Validate Membership Fee
            if (paidMembershipFee != membershipPlan.MembershipFee)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = $"Paid Membership Fee must be equal to {membershipPlan.MembershipFee}."
                });
            }

            // Check Duplicate Email
            bool emailExists = await _context.Users
                .AnyAsync(x => x.Email.ToLower() == email);

            if (emailExists)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Email already exists."
                });
            }

            // Check Duplicate Mobile Number
            bool mobileExists = await _context.Users
                .AnyAsync(x => x.MobileNumber == mobileNumber);

            if (mobileExists)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Mobile Number already exists."
                });
            }

            // Validate Age
            int age = DateTime.Today.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            if (age < 12)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "User must be at least 12 years old to register."
                });
            }

            // Generate User Id
            // Generate User Id
string? lastUserId = await _context.Users
    .OrderByDescending(x => x.UserId)
    .Select(x => x.UserId)
    .FirstOrDefaultAsync();

string userId = IdGenerator.GenerateUserId(lastUserId);

            // Membership Dates
            DateTime membershipStartDate = DateTime.Now;
            DateTime membershipEndDate =
                membershipStartDate.AddMonths(membershipPlan.DurationMonths);

                // Hash Password
string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Create User
            User user = new User
            {
                UserId = userId,
                FullName = fullName,
                Email = email,
                MobileNumber = mobileNumber,
                PasswordHash = hashedPassword,
                Address = address,
                DateOfBirth = dateOfBirth,
                Gender = gender,
                MembershipPlanId = membershipPlanId,
                PaidMembershipFee = paidMembershipFee,
                MembershipStartDate = membershipStartDate,
                MembershipEndDate = membershipEndDate,
                RegistrationStatus = registrationStatus,
                UserRole = UserRole.Member,
                CreatedDate = DateTime.Now
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = registrationStatus == RegistrationStatus.Pending
                    ? "Registration submitted successfully. Waiting for admin approval."
                    : "User registered successfully.",
                UserId = userId
            });
        }
        // Get Pending Registrations
        [Authorize(Roles = "Admin")]
[HttpGet("pending")]
public async Task<IActionResult> GetPendingRegistrations()
{
    var users = await _context.Users
        .Where(x => x.RegistrationStatus == RegistrationStatus.Pending)
        .Select(x => new
        {
            x.UserId,
            x.FullName,
            x.Email,
            x.MobileNumber,
            x.MembershipPlanId,
            x.CreatedDate
        })
        .ToListAsync();

    return Ok(users);
}

// Approve Registration
[Authorize(Roles = "Admin")]
[HttpPut("approve/{userId}")]
public async Task<IActionResult> ApproveRegistration(string userId)
{
    var user = await _context.Users
        .FirstOrDefaultAsync(x => x.UserId == userId);

    if (user == null)
    {
        return NotFound(new
        {
            Success = false,
            Message = "User not found."
        });
    }

    if (user.RegistrationStatus == RegistrationStatus.Approved)
    {
        return BadRequest(new
        {
            Success = false,
            Message = "User is already approved."
        });
    }

    user.RegistrationStatus = RegistrationStatus.Approved;

    await _context.SaveChangesAsync();

    return Ok(new
    {
        Success = true,
        Message = "User approved successfully."
    });
}

// Reject Registration
[Authorize(Roles = "Admin")]
[HttpPut("reject/{userId}")]
public async Task<IActionResult> RejectRegistration(
    string userId,
    RejectUserRequest request)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    var user = await _context.Users
        .FirstOrDefaultAsync(x => x.UserId == userId);

    if (user == null)
    {
        return NotFound(new
        {
            Success = false,
            Message = "User not found."
        });
    }

    user.RegistrationStatus = RegistrationStatus.Rejected;

    await _context.SaveChangesAsync();

    return Ok(new
    {
        Success = true,
        Message = "User registration rejected.",
        Reason = request.Reason
    });
}
// ---------------- LOGIN ----------------
[AllowAnonymous]
[HttpPost("login")]
public async Task<IActionResult> Login(LoginRequest request)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    // Trim Email
    string email = request.Email.Trim().ToLower();

    // Find User
    var user = await _context.Users
        .FirstOrDefaultAsync(x => x.Email.ToLower() == email);

    if (user == null)
    {
        return BadRequest(new
        {
            Success = false,
            Message = "Invalid Email or Password."
        });
    }

    // Check Registration Status
    if (user.RegistrationStatus == RegistrationStatus.Pending)
    {
        return BadRequest(new
        {
            Success = false,
            Message = "Your registration is still pending approval."
        });
    }

    if (user.RegistrationStatus == RegistrationStatus.Rejected)
    {
        return BadRequest(new
        {
            Success = false,
            Message = "Your registration has been rejected."
        });
    }

    // Verify Password
    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
        request.Password,
        user.PasswordHash);

    if (!isPasswordValid)
    {
        return BadRequest(new
        {
            Success = false,
            Message = "Invalid Email or Password."
        });
    }

    string token = _jwtHelper.GenerateToken(user);

return Ok(new
{
    Success = true,
    Message = "Login Successful.",
    Token = token,
    User = new
    {
        user.UserId,
        user.FullName,
        user.Email,
        Role = user.UserRole
    }
});
}
    }
}