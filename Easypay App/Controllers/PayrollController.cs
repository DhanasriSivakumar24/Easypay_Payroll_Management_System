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
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollService _payrollService;

        public PayrollController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }

        [HttpPost("generate")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<ActionResult> GeneratePayroll([FromBody] PayrollRequestDTO dto)
        {
            try
            {
                var result = await _payrollService.GeneratePayroll(dto);
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
        public async Task<ActionResult> GetAllPayrolls()
        {
            try
            {
                var result = await _payrollService.GetAllPayrolls();
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
