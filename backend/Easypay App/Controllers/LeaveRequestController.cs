using Easypay_App.Filters;
using Easypay_App.Interface;
using Easypay_App.Models.DTO;
using Easypay_App.Services;
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
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly IAuditTrailService _auditTrailService;

        public LeaveRequestController(
            ILeaveRequestService leaveRequestService,
            IAuditTrailService auditTrailService)
        {
            _leaveRequestService = leaveRequestService;
            _auditTrailService = auditTrailService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin, HR Manager, Manager")]
        public async Task<ActionResult> GetAllRequest()
        {
            try
            {
                var result = await _leaveRequestService.GetAllLeaveRequests();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Unable to Get all Leave Request Details");
            }
        }

        [HttpPost("submit")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult> SubmitLeave([FromBody] LeaveRequestDTO requestDTO)
        {
            try
            {
                var result = await _leaveRequestService.SubmitLeaveRequest(requestDTO);

                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                await _auditTrailService.LogAction(
                    User.Identity.Name,
                    actionId: 6,
                    entityName: "LeaveRequest",
                    entityId: result.Id,
                    oldValue: "-",
                    newValue: result,
                    ipAddress: ipAddress
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Unable to Submit Leave Request ");
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin, HR Manager, Employee,Manager")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var oldLeave = await _leaveRequestService.GetLeaveRequestById(id);
                var result = await _leaveRequestService.DeleteLeaveRequest(id);

                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                await _auditTrailService.LogAction(
                    User.Identity.Name,
                    actionId: 11,
                    entityName: "LeaveRequest",
                    entityId: id,
                    oldValue: oldLeave,
                    newValue: result,
                    ipAddress: ipAddress
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Delete Leave Request for Id: {id}");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, HR Manager, Manager, Employee")]
        public async Task<ActionResult> GetLeaveRequestById(int id)
        {
            try
            {
                var result = await _leaveRequestService.GetLeaveRequestById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Get Leave Request for Id: {id}");
            }
        }

        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Admin, HR Manager, Manager")]
        public async Task<ActionResult> ApproveLeave(int id, [FromQuery] int managerId)
        {
            try
            {
                var oldLeave = await _leaveRequestService.GetLeaveRequestById(id);
                var result = await _leaveRequestService.ApproveLeave(id, managerId, true);

                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                await _auditTrailService.LogAction(
                    User.Identity.Name,
                    actionId: 7,
                    entityName: "LeaveRequest",
                    entityId: id,
                    oldValue: oldLeave,
                    newValue: result,
                    ipAddress: ipAddress
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Approve Leave Request for Id: {id}");
            }
        }

        [HttpPut("reject/{id}")]
        [Authorize(Roles = "Admin, HR Manager, Manager")]
        public async Task<ActionResult> RejectLeave(int id, [FromQuery] int managerId)
        {
            try
            {
                var oldLeave = await _leaveRequestService.GetLeaveRequestById(id);
                var result = await _leaveRequestService.RejectLeave(id, managerId);

                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                await _auditTrailService.LogAction(
                    User.Identity.Name,
                    actionId: 8, 
                    entityName: "LeaveRequest",
                    entityId: id,
                    oldValue: oldLeave,
                    newValue: result,
                    ipAddress: ipAddress
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Reject Leave Request for Id: {id}");
            }
        }

        [HttpGet("Employee/{employeeId}")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult> GetLeaveRequestsByEmployee(int employeeId)
        {
            try
            {
                var result = await _leaveRequestService.GetLeaveRequestsByEmployee(employeeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Get Leave Requests for EmployeeId: {employeeId}");
            }
        }

    }
}
