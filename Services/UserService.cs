using LibraryManagementSystem.Data;
using LibraryManagementSystem.DTOs.UserResponseDto;
using LibraryManagementSystem.Enums;
using LibraryManagementSystem.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace LibraryManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly LibraryDbContext _context;

        public UserService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserResponseDto>> GetUsersAsync(RegistrationStatus? status)
        {
            var query = _context.Users
                .Include(x => x.MembershipPlan)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(x => x.RegistrationStatus == status.Value);
            }

            return await query
                .OrderBy(x => x.UserId)
                .Select(x => new UserResponseDto
                {
                    UserId = x.UserId,
                    FullName = x.FullName,
                    Email = x.Email,
                    MobileNumber = x.MobileNumber,
                    MembershipPlan = x.MembershipPlan!.MembershipName,
                    MembershipFee = x.PaidMembershipFee,
                    MembershipStartDate = x.MembershipStartDate,
                    MembershipEndDate = x.MembershipEndDate,
                    RegistrationStatus = x.RegistrationStatus.ToString(),
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();
        }

        public async Task<UserResponseDto> GetUserByIdAsync(string userId)
        {
            var user = await _context.Users
                .Include(x => x.MembershipPlan)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (user == null)
                throw new Exception("User not found.");

            return new UserResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                MembershipPlan = user.MembershipPlan!.MembershipName,
                MembershipFee = user.PaidMembershipFee,
                MembershipStartDate = user.MembershipStartDate,
                MembershipEndDate = user.MembershipEndDate,
                RegistrationStatus = user.RegistrationStatus.ToString(),
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate
            };
        }

        public async Task<List<UserResponseDto>> SearchUsersAsync(string keyword)
        {
            keyword = keyword.Trim().ToLower();

            return await _context.Users
                .Include(x => x.MembershipPlan)
                .Where(x =>
                    x.UserId.ToLower().Contains(keyword) ||
                    x.FullName.ToLower().Contains(keyword) ||
                    x.Email.ToLower().Contains(keyword) ||
                    x.MobileNumber.Contains(keyword) ||
                    x.RegistrationStatus.ToString().ToLower().Contains(keyword))
                .OrderBy(x => x.UserId)
                .Select(x => new UserResponseDto
                {
                    UserId = x.UserId,
                    FullName = x.FullName,
                    Email = x.Email,
                    MobileNumber = x.MobileNumber,
                    MembershipPlan = x.MembershipPlan!.MembershipName,
                    MembershipFee = x.PaidMembershipFee,
                    MembershipStartDate = x.MembershipStartDate,
                    MembershipEndDate = x.MembershipEndDate,
                    RegistrationStatus = x.RegistrationStatus.ToString(),
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();
        }
    }
}