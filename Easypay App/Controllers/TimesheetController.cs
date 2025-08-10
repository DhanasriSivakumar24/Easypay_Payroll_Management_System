using Easypay_App.Interface;
using Easypay_App.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easypay_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetController : ControllerBase
    {
        private readonly ITimesheetService _timesheetService;

        public TimesheetController(ITimesheetService timesheetService)
        {
            _timesheetService = timesheetService;
        }

        [HttpPost]
        public async Task<IActionResult> AddTimesheet([FromBody] TimesheetRequestDTO dto)
        {
            var result = await _timesheetService.AddTimesheet(dto);
            return Ok(result);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployee(int employeeId)
        {
            var result = await _timesheetService.GetTimesheetsByEmployee(employeeId);
            return Ok(result);
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            await _timesheetService.ApproveTimesheet(id);
            return Ok("Timesheet approved.");
        }

        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            await _timesheetService.RejectTimesheet(id);
            return Ok("Timesheet rejected.");
        }
    }
}
