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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddAuditTrail([FromBody] AuditTrailRequestDTO request)
        {
            if (request == null)
                return BadRequest("Invalid request data.");

            var result = await _auditTrailService.LogAction(request);
            return Ok(result);
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
