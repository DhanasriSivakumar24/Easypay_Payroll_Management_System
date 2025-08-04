using Easypay_App.Interface;
using Easypay_App.Models.DTO;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin, HR Manager")]
        public ActionResult GeneratePayroll([FromBody] PayrollRequestDTO dto)
        {
            try
            {
                var result = _payrollService.GeneratePayroll(dto);
                return Ok(result);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Unable to Generate Payroll");
            }
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin, HR Manager")]
        public ActionResult GetAllPayrolls()
        {
            try
            {
                var result = _payrollService.GetAllPayrolls();
                return Ok(result);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Unable to Get all Payroll");
            }
        }
    }
}
