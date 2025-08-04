using Easypay_App.Interface;
using Easypay_App.Models.DTO;
using Easypay_App.Services;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles ="Admin,HR Manager")]
        public ActionResult AddPolicy(PayrollPolicyAddRequestDTO dto)
        {
            try
            {
                var result = _payrollPolicyService.AddPolicy(dto);
                return Ok(result);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Add Policy");
            }
        }

        [Authorize(Roles = "Admin,HR Manager")]
        [HttpPut("update/{id}")]
        public ActionResult UpdatePolicy(int id, PayrollPolicyAddRequestDTO dto)
        {
            try
            {
                var result = _payrollPolicyService.UpdatePolicy(id, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Update Policy for Id: {id}");
            }
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin,HR Manager")]
        public ActionResult DeletePolicy(int id)
        {
            try
            {
                var result = _payrollPolicyService.DeletePolicy(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Delete Policy for Id: {id}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetPolicyById(int id)
        {
            try
            {
                var result = _payrollPolicyService.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Get Policy for Id: {id}");
            }
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin,HR Manager")]
        public ActionResult GetAll()
        {
            try
            {
                var result = _payrollPolicyService.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Get All Policy");
            }
        }

    }
}
