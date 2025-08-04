using Easypay_App.Interface;
using Easypay_App.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easypay_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BenefitEnrollmentController : ControllerBase
    {
        private readonly IBenefitEnrollmentService _benefitEnrollmentService;

        public BenefitEnrollmentController(IBenefitEnrollmentService benefitEnrollmentService)
        {
            _benefitEnrollmentService=benefitEnrollmentService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin, HR Manager")]
        public ActionResult GetAllEnrollment()
        {
            try
            {
                var response = _benefitEnrollmentService.GetAllBenefit();
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Unable to Get all Enrollment Details");
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetEnrollmentById(int id)
        {
            try
            {
                var response = _benefitEnrollmentService.GetBenefitById(id);
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
        public ActionResult AddEnrollment(BenefitEnrollmentAddRequestDTO requestDTO)
        {
            try
            {
                var enrollment = _benefitEnrollmentService.EnrollBenefit(requestDTO);
                return Ok(enrollment);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Add Benefit Enrollment");
            }

        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin, HR Manager")]
        public ActionResult UpdateEnrollment(int id ,BenefitEnrollmentAddRequestDTO requestDTO)
        {
            try
            {
                var emrollment = _benefitEnrollmentService.UpdateBenefit(id, requestDTO);
                return Ok(emrollment);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Update Benefit Enrollment for Id: {id}");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, HR Manager")]
        public ActionResult DeleteEnrollment(int id)
        {
            try
            {
                var emrollment = _benefitEnrollmentService.DeleteBenefit(id);
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
