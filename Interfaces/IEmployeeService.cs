using LibraryManagementSystem.DTOs.Employee;
using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeResponseDto>> GetEmployeesAsync(StaffRole? role);

        Task<EmployeeResponseDto> GetEmployeeByIdAsync(string employeeId);

        Task<string> UpdateEmployeeAsync(string employeeId, UpdateEmployeeRequestDto request);

        Task<string> ChangeEmployeeStatusAsync(string employeeId, ChangeEmployeeStatusRequestDto request);

        Task<List<EmployeeResponseDto>> SearchEmployeesAsync(string keyword);
    }
}