using LibraryManagementSystem.Data;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Enums;
using LibraryManagementSystem.Helpers;
using LibraryManagementSystem.Models;
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

        // ---------------- SELF REGISTRATION ----------------

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
                UserRole.Member,
                RegistrationStatus.Pending);
        }

        // ---------------- ADMIN REGISTRATION ----------------

        [HttpPost("admin/register")]
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
                request.UserRole,
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
            UserRole userRole,
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
                return BadRequest("Invalid Membership Plan.");
            }

            // Validate Membership Fee
            if (paidMembershipFee != membershipPlan.MembershipFee)
            {
                return BadRequest("Membership Fee does not match the selected Membership Plan.");
            }

            // Check Duplicate Email
            bool emailExists = await _context.Users
                .AnyAsync(x => x.Email.ToLower() == email);

            if (emailExists)
            {
                return BadRequest("Email already exists.");
            }

            // Check Duplicate Mobile Number
            bool mobileExists = await _context.Users
                .AnyAsync(x => x.MobileNumber == mobileNumber);

            if (mobileExists)
            {
                return BadRequest("Mobile Number already exists.");
            }

            // Validate Age
            int age = DateTime.Today.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            if (age < 12)
            {
                return BadRequest("User must be at least 12 years old.");
            }

            // Generate User Id
            int userCount = await _context.Users.CountAsync();
            string userId = IdGenerator.GenerateUserId(userCount);

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
                UserRole = userRole,
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
[HttpPut("approve/{userId}")]
public async Task<IActionResult> ApproveRegistration(string userId)
{
    var user = await _context.Users
        .FirstOrDefaultAsync(x => x.UserId == userId);

    if (user == null)
    {
        return NotFound("User not found.");
    }

    if (user.RegistrationStatus == RegistrationStatus.Approved)
    {
        return BadRequest("User is already approved.");
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
        return NotFound("User not found.");
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
    }
}