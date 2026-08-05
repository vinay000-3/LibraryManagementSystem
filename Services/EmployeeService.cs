using LibraryManagementSystem.Data;
using LibraryManagementSystem.DTOs.Employee;
using LibraryManagementSystem.Interfaces;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly LibraryDbContext _context;

        public EmployeeService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeResponseDto>> GetEmployeesAsync(StaffRole? role)
{
    var query = _context.LibraryEmployees.AsQueryable();

    if (role.HasValue)
    {
        query = query.Where(x => x.Role == role.Value);
    }

    var employees = await query
        .OrderBy(x => x.EmployeeId)
        .Select(x => new EmployeeResponseDto
        {
            EmployeeId = x.EmployeeId,
            FullName = x.FullName,
            Email = x.Email,
            Role = x.Role.ToString(),
            Status = x.Status.ToString(),
            CreatedDate = x.CreatedDate
        })
        .ToListAsync();

    return employees;
}

        public async Task<EmployeeResponseDto> GetEmployeeByIdAsync(string employeeId)
{
    var employee = await _context.LibraryEmployees
        .FirstOrDefaultAsync(x => x.EmployeeId == employeeId);

    if (employee == null)
        throw new Exception("Employee not found.");

    return new EmployeeResponseDto
    {
        EmployeeId = employee.EmployeeId,
        FullName = employee.FullName,
        Email = employee.Email,
        Role = employee.Role.ToString(),
        Status = employee.Status.ToString(),
        CreatedDate = employee.CreatedDate
    };
}
public async Task<string> UpdateEmployeeAsync(string employeeId, UpdateEmployeeRequestDto request)
{
    var employee = await _context.LibraryEmployees
        .FirstOrDefaultAsync(x => x.EmployeeId == employeeId);

    if (employee == null)
        throw new Exception("Employee not found.");

    bool emailExists = await _context.LibraryEmployees.AnyAsync(x =>
        x.Email == request.Email &&
        x.EmployeeId != employeeId);

    if (emailExists)
        throw new Exception("Email already exists.");

    employee.FullName = request.FullName;
    employee.Email = request.Email;

    await _context.SaveChangesAsync();

    return "Employee updated successfully.";
}

        public async Task<string> ChangeEmployeeStatusAsync(string employeeId, ChangeEmployeeStatusRequestDto request)
{
    var employee = await _context.LibraryEmployees
        .FirstOrDefaultAsync(x => x.EmployeeId == employeeId);

    if (employee == null)
        throw new Exception("Employee not found.");

    employee.Status = request.Status;

    await _context.SaveChangesAsync();

    return $"Employee status changed to {request.Status}.";
}

public async Task<List<EmployeeResponseDto>> SearchEmployeesAsync(string keyword)
{
    keyword = keyword.Trim().ToLower();

    var employees = await _context.LibraryEmployees
        .Where(x =>
            x.EmployeeId.ToLower().Contains(keyword) ||
            x.FullName.ToLower().Contains(keyword) ||
            x.Email.ToLower().Contains(keyword) ||
            x.Role.ToString().ToLower().Contains(keyword) ||
            x.Status.ToString().ToLower().Contains(keyword))
        .OrderBy(x => x.EmployeeId)
        .Select(x => new EmployeeResponseDto
        {
            EmployeeId = x.EmployeeId,
            FullName = x.FullName,
            Email = x.Email,
            Role = x.Role.ToString(),
            Status = x.Status.ToString(),
            CreatedDate = x.CreatedDate
        })
        .ToListAsync();

    return employees;
}
    }
}