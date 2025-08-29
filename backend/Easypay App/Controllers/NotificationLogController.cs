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
    public class NotificationLogController : ControllerBase
    {
        private readonly INotificationLogService _service;
        private readonly IAuditTrailService _auditTrailService;

        public NotificationLogController(
            INotificationLogService service,
            IAuditTrailService auditTrailService)
        {
            _service = service;
            _auditTrailService = auditTrailService;
        }

        [HttpPost("send")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<ActionResult<NotificationLogDTO>> SendNotification(NotificationLogRequestDTO request)
        {
            try
            {
                var result = await _service.SendNotification(request);
                if (result == null)
                    return BadRequest("Notification could not be sent. Invalid user/channel or internal error.");

                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                await _auditTrailService.LogAction(
                    User.Identity?.Name,
                    actionId: 12,
                    entityName: "NotificationLog",
                    entityId: result.Id,
                    oldValue: "N/A",
                    newValue: result,
                    ipAddress: ipAddress
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the error using ILogger
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin, HR Manager, Employee")]
        public async Task<ActionResult<IEnumerable<NotificationLogDTO>>> GetByUser(int userId)
        {
            try
            {
                var result = await _service.GetNotificationsByUser(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to Get Notification Log by UserId: {userId}");
            }
        }
    }
}
