using Easypay_App.Filters;
using Easypay_App.Interface;
using Easypay_App.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easypay_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomExceptionFilter]
    public class BenefitEnrollmentController : ControllerBase
    {
        private readonly IBenefitEnrollmentService _benefitEnrollmentService;

        public BenefitEnrollmentController(IBenefitEnrollmentService benefitEnrollmentService)
        {
            _benefitEnrollmentService = benefitEnrollmentService;
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
                var emrollment = await _benefitEnrollmentService.UpdateBenefit(id, requestDTO);
                return Ok(emrollment);
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
                var emrollment = await _benefitEnrollmentService.DeleteBenefit(id);
                return Ok(emrollment);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Delete Enrollment for Id: {id}");
            }
        }
    }
}