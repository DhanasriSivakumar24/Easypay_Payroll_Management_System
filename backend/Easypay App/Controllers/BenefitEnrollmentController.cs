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
    public class BenefitEnrollmentController : ControllerBase
    {
        private readonly IBenefitEnrollmentService _benefitEnrollmentService;
        private readonly IAuditTrailService _auditTrailService;

        public BenefitEnrollmentController(IBenefitEnrollmentService benefitEnrollmentService,
            IAuditTrailService auditTrailService
            )
        {
            _benefitEnrollmentService = benefitEnrollmentService;
            _auditTrailService = auditTrailService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<ActionResult> GetAllEnrollment()
        {
            try
            {
                var response = await _benefitEnrollmentService.GetAllBenefit();
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return BadRequest(new { errorNumber = 500, errorMessage = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, HR Manager, Employee")]
        public async Task<ActionResult> GetEnrollmentById(int id)
        {
            try
            {
                var response = await _benefitEnrollmentService.GetBenefitById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Get Enrollment {id}");
            }
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<ActionResult> AddEnrollment(BenefitEnrollmentAddRequestDTO requestDTO)
        {
            try
            {
                var enrollment = await _benefitEnrollmentService.EnrollBenefit(requestDTO);
                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                await _auditTrailService.LogAction(
                    User.Identity.Name,
                    actionId: 16,
                    entityName: "BenefitEnrollment",
                    entityId: enrollment.Id,
                    oldValue: "-",
                    newValue: enrollment,
                    ipAddress: ipAddress
                );
                return Ok(enrollment);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return BadRequest(new { errorNumber = 500, errorMessage = ex.Message });
            }

        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<ActionResult> UpdateEnrollment(int id, BenefitEnrollmentAddRequestDTO requestDTO)
        {
            try
            {
                var oldEnrollment = await _benefitEnrollmentService.GetBenefitById(id); // old values
                var updatedEnrollment = await _benefitEnrollmentService.UpdateBenefit(id, requestDTO);
                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                await _auditTrailService.LogAction(
                    User.Identity.Name,
                    actionId: 17,
                    entityName: "BenefitEnrollment",
                    entityId: id,
                    oldValue: oldEnrollment,
                    newValue: updatedEnrollment,
                    ipAddress:ipAddress
                );
                return Ok(updatedEnrollment);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return BadRequest(new { errorNumber = 500, errorMessage = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<ActionResult> DeleteEnrollment(int id)
        {
            try
            {
                var deletedEnrollment = await _benefitEnrollmentService.DeleteBenefit(id);
                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                await _auditTrailService.LogAction(
                    User.Identity.Name,
                    actionId: 18, 
                    entityName: "Benefit Enrollment",
                    entityId: id,
                    oldValue: deletedEnrollment,
                    newValue: "-",
                    ipAddress:ipAddress
                );
                return Ok(deletedEnrollment);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Delete Enrollment for Id: {id}");
            }
        }
    }
}