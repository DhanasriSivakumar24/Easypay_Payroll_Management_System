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
    public class PayrollPolicyController : ControllerBase
    {
        private readonly IPayrollPolicyService _payrollPolicyService;
        private readonly IAuditTrailService _auditTrailService;

        public PayrollPolicyController(
            IPayrollPolicyService payrollPolicyService,
            IAuditTrailService auditTrailService)
        {
            _payrollPolicyService = payrollPolicyService;
            _auditTrailService = auditTrailService;
        }

        [HttpPost("add")]
        [Authorize(Roles ="Admin,HR Manager")]
        public async Task<ActionResult> AddPolicy(PayrollPolicyAddRequestDTO dto)
        {
            try
            {
                var result = await _payrollPolicyService.AddPolicy(dto);

                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                await _auditTrailService.LogAction(
                    User.Identity?.Name ?? "Unknown",
                    actionId: 23, // Payroll Policy Created
                    entityName: "PayrollPolicy",
                    entityId: result.Id,
                    oldValue: "N/A",
                    newValue: result,
                    ipAddress: ipAddress
                );
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
        public async Task<ActionResult> UpdatePolicy(int id, PayrollPolicyAddRequestDTO dto)
        {
            try
            {
                var oldPolicy = await _payrollPolicyService.GetById(id);
                var result = await _payrollPolicyService.UpdatePolicy(id, dto);

                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                await _auditTrailService.LogAction(
                    User.Identity?.Name ?? "Unknown",
                    actionId: 24, // Payroll Policy Updated
                    entityName: "PayrollPolicy",
                    entityId: id,
                    oldValue: oldPolicy,
                    newValue: result,
                    ipAddress: ipAddress
                );
                
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
        public async Task<ActionResult> DeletePolicy(int id)
        {
            try
            {
                var oldPolicy = await _payrollPolicyService.GetById(id);
                var result = await _payrollPolicyService.DeletePolicy(id);

                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                await _auditTrailService.LogAction(
                    User.Identity?.Name ?? "Unknown",
                    actionId: 25, // Payroll Policy Deleted
                    entityName: "PayrollPolicy",
                    entityId: id,
                    oldValue: oldPolicy,
                    newValue: "Deleted",
                    ipAddress: ipAddress
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Delete Policy for Id: {id}");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<ActionResult> GetPolicyById(int id)
        {
            try
            {
                var result = await _payrollPolicyService.GetById(id);
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
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var result = await _payrollPolicyService.GetAll();
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
