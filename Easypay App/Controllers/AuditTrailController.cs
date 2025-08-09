using Easypay_App.Interface;
using Easypay_App.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easypay_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditTrailController : ControllerBase
    {
        private readonly IAuditTrailService _auditTrailService;

        public AuditTrailController(IAuditTrailService auditTrailService)
        {
            _auditTrailService = auditTrailService;
        }

        #region Add Audit Trail
        [HttpPost]
        public IActionResult AddAuditTrail([FromBody] AuditTrailRequestDTO request)
        {
            if (request == null)
                return BadRequest("Invalid request data.");

            var result = _auditTrailService.LogAction(request);
            return Ok(result);
        }
        #endregion

        #region Get All
        [HttpGet]
        public IActionResult GetAllAuditTrails()
        {
            var logs = _auditTrailService.GetAllLogs();
            return Ok(logs);
        }
        #endregion

        #region Get By Id
        [HttpGet("{id}")]
        public IActionResult GetAuditTrailById(int id)
        {
            var log = _auditTrailService.GetLogById(id);
            if (log == null)
                return NotFound($"Audit trail with ID {id} not found.");

            return Ok(log);
        }
        #endregion

        #region Get By User
        [HttpGet("user/{userId}")]
        public IActionResult GetAuditTrailsByUser(int userId)
        {
            var logs = _auditTrailService.GetLogsByUser(userId);
            return Ok(logs);
        }
        #endregion
    }
}
