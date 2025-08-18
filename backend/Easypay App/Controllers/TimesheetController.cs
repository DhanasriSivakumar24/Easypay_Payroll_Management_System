using Easypay_App.Filters;
using Easypay_App.Interface;
using Easypay_App.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easypay_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomExceptionFilter]
    [EnableCors("DefaultCORS")]
    public class TimesheetController : ControllerBase
    {
        private readonly ITimesheetService _timesheetService;

        public TimesheetController(ITimesheetService timesheetService)
        {
            _timesheetService = timesheetService;
        }

        [HttpPost]
        [Authorize(Roles = "Employee, HR Manager")]
        public async Task<IActionResult> AddTimesheet([FromBody] TimesheetRequestDTO dto)
        {
            var result = await _timesheetService.AddTimesheet(dto);
            return Ok(result);
        }

        [HttpGet("employee/{employeeId}")]
        [Authorize(Roles = "Admin, HR Manager, Employee")]
        public async Task<IActionResult> GetByEmployee(int employeeId)
        {
            var result = await _timesheetService.GetTimesheetsByEmployee(employeeId);
            return Ok(result);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<IActionResult> Approve(int id)
        {
            await _timesheetService.ApproveTimesheet(id);
            return Ok("Timesheet approved.");
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<IActionResult> Reject(int id)
        {
            await _timesheetService.RejectTimesheet(id);
            return Ok("Timesheet rejected.");
        }
    }
}
