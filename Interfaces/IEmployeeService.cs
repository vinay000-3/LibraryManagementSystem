using LibraryManagementSystem.DTOs.Employee;

namespace LibraryManagementSystem.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeResponseDto>> GetAllEmployeesAsync();

        Task<EmployeeResponseDto> GetEmployeeByIdAsync(string employeeId);

        Task<string> UpdateEmployeeAsync(string employeeId, UpdateEmployeeRequestDto request);

        Task<string> ChangeEmployeeStatusAsync(string employeeId, ChangeEmployeeStatusRequestDto request);
    }
}