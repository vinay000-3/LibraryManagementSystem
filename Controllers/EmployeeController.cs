using LibraryManagementSystem.DTOs.Employee;
using LibraryManagementSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var result = await _employeeService.GetAllEmployeesAsync();
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

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployeeById(string employeeId)
        {
            try
            {
                var result = await _employeeService.GetEmployeeByIdAsync(employeeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(
            string employeeId,
            UpdateEmployeeRequestDto request)
        {
            try
            {
                var result = await _employeeService.UpdateEmployeeAsync(employeeId, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{employeeId}/status")]
        public async Task<IActionResult> ChangeEmployeeStatus(
            string employeeId,
            ChangeEmployeeStatusRequestDto request)
        {
            try
            {
                var result = await _employeeService.ChangeEmployeeStatusAsync(employeeId, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}