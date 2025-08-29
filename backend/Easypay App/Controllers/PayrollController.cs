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
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollService _payrollService;
        private readonly IAuditTrailService _auditTrailService;

        public PayrollController(IPayrollService payrollService, 
            IAuditTrailService auditTrailService)
        {
            _payrollService = payrollService;
            _auditTrailService = auditTrailService;
        }

        [HttpPost("generate")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<ActionResult<PayrollResponseDTO>> GeneratePayroll([FromBody] PayrollRequestDTO dto)
        {
            var result = await _payrollService.GeneratePayroll(dto);
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            await _auditTrailService.LogAction(
                User.Identity.Name,
                actionId: 9, // Run Payroll
                entityName: "Payroll",
                entityId: result.Id,
                oldValue: "-",
                newValue: result,
                ipAddress: ipAddress
            );
            return Ok(result);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin, HR Manager, Payroll Processor")]
        public async Task<ActionResult<IEnumerable<PayrollResponseDTO>>> GetAllPayrolls()
        {
            var result = await _payrollService.GetAllPayrolls();
            return Ok(result);
        }

        [HttpGet("employee/{empId}")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<IEnumerable<PayrollResponseDTO>>> GetPayrollByEmployeeId(int empId)
        {
            var result = await _payrollService.GetPayrollByEmployeeId(empId);
            return Ok(result);
        }

        [HttpPut("verify/{payrollId}")]
        [Authorize(Roles = "Payroll Processor")]
        public async Task<ActionResult<PayrollResponseDTO>> VerifyPayroll(int payrollId)
        {
            var oldPayroll = await _payrollService.GetPayrollById(payrollId);
            var result = await _payrollService.VerifyPayroll(payrollId);
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            await _auditTrailService.LogAction(
                User.Identity.Name,
                actionId: 20,
                entityName: "Payroll",
                entityId: payrollId,
                oldValue: oldPayroll,
                newValue: result,
                ipAddress: ipAddress
            );
            return Ok(result);
        }

        [HttpPut("approve/{payrollId}")]
        [Authorize(Roles = "HR Manager")]
        public async Task<ActionResult<PayrollResponseDTO>> ApprovePayroll(int payrollId)
        {
            var oldPayroll = await _payrollService.GetPayrollById(payrollId);
            var result = await _payrollService.ApprovePayroll(payrollId);
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            await _auditTrailService.LogAction(
                User.Identity.Name,
                actionId: 19,
                entityName: "Payroll",
                entityId: payrollId,
                oldValue: oldPayroll,
                newValue: result,
                ipAddress: ipAddress
            );
            return Ok(result);
        }

        [HttpPut("mark-paid/{payrollId}/{adminId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PayrollResponseDTO>> MarkAsPaid(int payrollId, int adminId)
        {
            var oldPayroll = await _payrollService.GetPayrollById(payrollId);
            var result = await _payrollService.MarkPayrollAsPaid(payrollId, adminId);
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            await _auditTrailService.LogAction(
                User.Identity.Name,
                actionId: 21,
                entityName: "Payroll",
                entityId: payrollId,
                oldValue: oldPayroll,
                newValue: result,
                ipAddress: ipAddress
            );
            return Ok(result);
        }

        [HttpGet("get-approved-payroll")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<ActionResult<IEnumerable<PayrollResponseDTO>>> GetApprovedPayrolls([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var result = await _payrollService.GetApprovedPayrolls(start, end);
            return Ok(result);
        }

        [HttpGet("compliance-report")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<ActionResult<ComplianceReportDTO>> GetComplianceReport([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            if (start == default || end == default)
                return BadRequest("Please provide valid start and end dates.");

            if (end < start)
                return BadRequest("End date must be after start date.");

            var report = await _payrollService.GenerateComplianceReport(start, end);

            if (report == null || !report.EmployeeDetails.Any())
                return NotFound("No approved payrolls found for the given period.");

            return Ok(report);
        }

        [HttpGet("{payrollId}")]
        [Authorize(Roles = "Admin, HR Manager, Payroll Processor, Employee")]
        public async Task<ActionResult<PayrollResponseDTO>> GetPayrollById(int payrollId)
        {
            var payroll = await _payrollService.GetPayrollById(payrollId);
            return Ok(payroll);
        }
    }
}
