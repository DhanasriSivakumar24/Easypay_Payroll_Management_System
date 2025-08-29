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
        private readonly IAuditTrailService _auditTrailService;

        public TimesheetController(
            ITimesheetService timesheetService,
            IAuditTrailService auditTrailService)
        {
            _timesheetService = timesheetService;
            _auditTrailService = auditTrailService;
        }

        [HttpPost]
        [Authorize(Roles = "Employee, HR Manager")]
        public async Task<IActionResult> AddTimesheet([FromBody] TimesheetRequestDTO dto)
        {
            var result = await _timesheetService.AddTimesheet(dto);

            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            await _auditTrailService.LogAction(
                User.Identity?.Name ?? "Unknown",
                actionId: 13, // Apply Timesheet
                entityName: "Timesheet",
                entityId: result.Id,
                oldValue: "N/A",
                newValue: result,
                ipAddress: ipAddress
            );
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

            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            await _auditTrailService.LogAction(
                User.Identity?.Name ?? "Unknown",
                actionId: 15, // Approve Timesheet
                entityName: "Timesheet",
                entityId: id,
                oldValue: "Pending",
                newValue: "Approved",
                ipAddress: ipAddress
            );
            return Ok("Timesheet approved.");
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<IActionResult> Reject(int id)
        {
            await _timesheetService.RejectTimesheet(id);

            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            await _auditTrailService.LogAction(
                User.Identity?.Name ?? "Unknown",
                actionId: 14, // Reject Timesheet
                entityName: "Timesheet",
                entityId: id,
                oldValue: "Pending",
                newValue: "Rejected",
                ipAddress: ipAddress
            );
            return Ok("Timesheet rejected.");
        }
    }
}
