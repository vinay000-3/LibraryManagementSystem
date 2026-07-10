using LibraryManagementSystem.Data;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public MembershipController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Membership Controller is Working");
        }

        
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateMembershipPlanRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            var existingPlan = await _context.MembershipPlans
                .FirstOrDefaultAsync(x => x.MembershipName == request.MembershipName);

            if (existingPlan != null)
            {
                return BadRequest("Membership Plan already exists.");
            }

            
            string membershipPlanId = "MP0001";

            var lastPlan = await _context.MembershipPlans
                .OrderByDescending(x => x.MembershipPlanId)
                .FirstOrDefaultAsync();

            if (lastPlan != null)
            {
                int number = int.Parse(lastPlan.MembershipPlanId.Substring(2));
                membershipPlanId = "MP" + (number + 1).ToString("D4");
            }

            

            decimal monthlyOperationalCost =
                request.MaintenanceCost +
                request.StaffSalary +
                request.ElectricityCost +
                request.MiscellaneousExpenses;

            decimal monthlyCostPerMember =
                monthlyOperationalCost / request.ExpectedMembers;

            decimal durationCharge =
                monthlyCostPerMember * request.DurationMonths;

            decimal borrowingCharge =
                request.MaximumBooksAllowed * 75;

            decimal reservationCharge =
                request.MaximumReservationsAllowed * 40;

            decimal collectionCharge =
                request.NumberOfBookCopies * 2;

            decimal baseAmount =
                durationCharge +
                borrowingCharge +
                reservationCharge +
                collectionCharge;

            decimal amountWithProfit =
                baseAmount * (1 + request.ProfitPercentage / 100);

            decimal amountAfterDiscount =
                amountWithProfit * (1 - request.MembershipDiscountPercentage / 100);

            decimal membershipFee =
                amountAfterDiscount * (1 + request.GSTPercentage / 100);

            membershipFee = Math.Round(membershipFee, 2);

            
            var membershipPlan = new MembershipPlan
            {
                MembershipPlanId = membershipPlanId,
                MembershipName = request.MembershipName,
                DurationMonths = request.DurationMonths,
                MaximumBooksAllowed = request.MaximumBooksAllowed,
                MaximumReservationsAllowed = request.MaximumReservationsAllowed,
                MaintenanceCost = request.MaintenanceCost,
                StaffSalary = request.StaffSalary,
                ElectricityCost = request.ElectricityCost,
                MiscellaneousExpenses = request.MiscellaneousExpenses,
                NumberOfBooks = request.NumberOfBooks,
                NumberOfBookCopies = request.NumberOfBookCopies,
                ExpectedMembers = request.ExpectedMembers,
                ProfitPercentage = request.ProfitPercentage,
                GSTPercentage = request.GSTPercentage,
                MembershipDiscountPercentage = request.MembershipDiscountPercentage,
                MembershipFee = membershipFee,
                CreatedDate = DateTime.Now
            };

            _context.MembershipPlans.Add(membershipPlan);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = "Membership Plan Created Successfully",
                MembershipPlanId = membershipPlanId,
                MembershipFee = membershipFee
            });
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAllMembershipPlans()
        {
            var membershipPlans = await _context.MembershipPlans.ToListAsync();

            return Ok(membershipPlans);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMembershipById(string id)
        {
            var membershipPlan = await _context.MembershipPlans
                .FirstOrDefaultAsync(x => x.MembershipPlanId == id);

            if (membershipPlan == null)
            {
                return NotFound("Membership Plan not found.");
            }

            return Ok(membershipPlan);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMembershipPlan(string id, UpdateMembershipPlanRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var membershipPlan = await _context.MembershipPlans
                .FirstOrDefaultAsync(x => x.MembershipPlanId == id);

            if (membershipPlan == null)
            {
                return NotFound("Membership Plan not found.");
            }

            var existingPlan = await _context.MembershipPlans
                .FirstOrDefaultAsync(x =>
                    x.MembershipName == request.MembershipName &&
                    x.MembershipPlanId != id);

            if (existingPlan != null)
            {
                return BadRequest("Another Membership Plan with this name already exists.");
            }

           

            decimal monthlyOperationalCost =
                request.MaintenanceCost +
                request.StaffSalary +
                request.ElectricityCost +
                request.MiscellaneousExpenses;

            decimal monthlyCostPerMember =
                monthlyOperationalCost / request.ExpectedMembers;

            decimal durationCharge =
                monthlyCostPerMember * request.DurationMonths;

            decimal borrowingCharge =
                request.MaximumBooksAllowed * 75;

            decimal reservationCharge =
                request.MaximumReservationsAllowed * 40;

            decimal collectionCharge =
                request.NumberOfBookCopies * 2;

            decimal baseAmount =
                durationCharge +
                borrowingCharge +
                reservationCharge +
                collectionCharge;

            decimal amountWithProfit =
                baseAmount * (1 + request.ProfitPercentage / 100);

            decimal amountAfterDiscount =
                amountWithProfit * (1 - request.MembershipDiscountPercentage / 100);

            decimal membershipFee =
                amountAfterDiscount * (1 + request.GSTPercentage / 100);

            membershipFee = Math.Round(membershipFee, 2);

            
            membershipPlan.MembershipName = request.MembershipName;
            membershipPlan.DurationMonths = request.DurationMonths;
            membershipPlan.MaximumBooksAllowed = request.MaximumBooksAllowed;
            membershipPlan.MaximumReservationsAllowed = request.MaximumReservationsAllowed;
            membershipPlan.MaintenanceCost = request.MaintenanceCost;
            membershipPlan.StaffSalary = request.StaffSalary;
            membershipPlan.ElectricityCost = request.ElectricityCost;
            membershipPlan.MiscellaneousExpenses = request.MiscellaneousExpenses;
            membershipPlan.NumberOfBooks = request.NumberOfBooks;
            membershipPlan.NumberOfBookCopies = request.NumberOfBookCopies;
            membershipPlan.ExpectedMembers = request.ExpectedMembers;
            membershipPlan.ProfitPercentage = request.ProfitPercentage;
            membershipPlan.GSTPercentage = request.GSTPercentage;
            membershipPlan.MembershipDiscountPercentage = request.MembershipDiscountPercentage;
            membershipPlan.MembershipFee = membershipFee;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = "Membership Plan Updated Successfully",
                MembershipPlanId = membershipPlan.MembershipPlanId,
                MembershipFee = membershipFee
            });
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMembershipPlan(string id)
        {
            var membershipPlan = await _context.MembershipPlans
                .FirstOrDefaultAsync(x => x.MembershipPlanId == id);

            if (membershipPlan == null)
            {
                return NotFound("Membership Plan not found.");
            }

            _context.MembershipPlans.Remove(membershipPlan);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = "Membership Plan Deleted Successfully"
            });
        }
    }
}