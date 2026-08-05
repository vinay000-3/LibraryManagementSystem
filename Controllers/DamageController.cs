using LibraryManagementSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DamageController : ControllerBase
    {
        private readonly IDamageService _damageService;

        public DamageController(IDamageService damageService)
        {
            _damageService = damageService;
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingDamagedBooks()
        {
            var result = await _damageService.GetPendingDamagedBooksAsync();

            return Ok(result);
        }

        [HttpPut("send-for-repair/{damageRecordId}")]
        public async Task<IActionResult> SendForRepair(string damageRecordId)
        {
            var message = await _damageService.SendForRepairAsync(damageRecordId);

            return Ok(new
            {
                Success = true,
                Message = message
            });
        }

        [HttpPut("dispose/{damageRecordId}")]
        public async Task<IActionResult> DisposeBook(string damageRecordId)
        {
            var message = await _damageService.DisposeBookAsync(damageRecordId);

            return Ok(new
            {
                Success = true,
                Message = message
            });
        }

        [HttpPut("repair-completed/{damageRecordId}")]
        public async Task<IActionResult> RepairCompleted(string damageRecordId)
        {
            var message = await _damageService.RepairCompletedAsync(damageRecordId);

            return Ok(new
            {
                Success = true,
                Message = message
            });
        }
    }
}