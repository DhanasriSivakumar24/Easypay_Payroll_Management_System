using Easypay_App.Interface;
using Easypay_App.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easypay_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollService _payrollService;

        public PayrollController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }

        [HttpPost("generate")]
        public ActionResult GeneratePayroll([FromBody] PayrollRequestDTO dto)
        {
            var result = _payrollService.GeneratePayroll(dto);
            return Ok(result);
        }

        [HttpGet("all")]
        public ActionResult GetAllPayrolls()
        {
            var result = _payrollService.GetAllPayrolls();
            return Ok(result);
        }
    }
}
