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
    public class AuditTrailController : ControllerBase
    {
        private readonly IAuditTrailService _auditTrailService;

        public AuditTrailController(IAuditTrailService auditTrailService)
        {
            _auditTrailService = auditTrailService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<ActionResult<IEnumerable<AuditTrailResponseDTO>>> GetAllLogs()
        {
            var logs = await _auditTrailService.GetAllLogs();
            return Ok(logs);
        }

        [HttpGet("user/{userName}")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<ActionResult<IEnumerable<AuditTrailResponseDTO>>> GetLogsByUser(string userName)
        {
            var logs = await _auditTrailService.GetLogsByUser(userName);
            return Ok(logs);
        }

        [HttpGet("action/{actionId}")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<ActionResult<IEnumerable<AuditTrailResponseDTO>>> GetLogsByAction(int actionId)
        {
            var logs = await _auditTrailService.GetLogsByAction(actionId);
            return Ok(logs);
        }

        [Authorize(Roles = "Admin, HR Manager")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuditTrailById(int id)
        {
            var log = await _auditTrailService.GetLogById(id);
            if (log == null)
                return NotFound($"Audit trail with ID {id} not found.");

            return Ok(log);
        }
    }
}
