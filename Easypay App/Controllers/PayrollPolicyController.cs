using Easypay_App.Interface;
using Easypay_App.Models.DTO;
using Easypay_App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easypay_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollPolicyController : ControllerBase
    {
        private readonly IPayrollPolicyService _payrollPolicyService;

        public PayrollPolicyController(IPayrollPolicyService payrollPolicyService)
        {
            _payrollPolicyService= payrollPolicyService;
        }
        [HttpPost("add")]
        public ActionResult AddPolicy(PayrollPolicyRequestDTO dto)
        {
            var result = _payrollPolicyService.AddPolicy(dto);
            return Ok(result);
        }
        [HttpPut("update/{id}")]
        public ActionResult UpdatePolicy(int id, PayrollPolicyRequestDTO dto)
        {
            var result = _payrollPolicyService.UpdatePolicy(id,dto);
            return Ok(result);
        }
        [HttpDelete("Delete/{id}")]
        public ActionResult DeletePolicy(int id)
        {
            var result = _payrollPolicyService.DeletePolicy(id);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public ActionResult GetPolicyById(int id)
        {
            var result = _payrollPolicyService.GetById(id);
            return Ok(result);
        }
        [HttpGet("all")]
        public ActionResult GetAll()
        {
            var result = _payrollPolicyService.GetAll();
            return Ok(result);
        }

    }
}
