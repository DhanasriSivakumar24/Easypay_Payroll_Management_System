using Easypay_App.Interface;
using Easypay_App.Models.DTO;
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
        public ActionResult GetAllEnrollment()
        {
            var response = _benefitEnrollmentService.GetAllBenefit();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult GetEnrollmentById(int id)
        {
            var response = _benefitEnrollmentService.GetBenefitById(id);
            return Ok(response);
        }

        [HttpPost("add")]
        public ActionResult AddEnrollment(BenefitEnrollmentAddRequestDTO requestDTO)
        {
            var enrollment = _benefitEnrollmentService.EnrollBenefit(requestDTO);
            return Ok(enrollment);
        }
        [HttpPut("update/{id}")]
        public ActionResult UpdateEnrollment(int id ,BenefitEnrollmentAddRequestDTO requestDTO)
        {
            var emrollment =_benefitEnrollmentService.UpdateBenefit(id, requestDTO);
            return Ok(emrollment);
        }
        [HttpDelete]
        public ActionResult DeleteEnrollment(int id)
        {
            var emrollment = _benefitEnrollmentService.DeleteBenefit(id);
            return Ok(emrollment);
        }
    }
}
