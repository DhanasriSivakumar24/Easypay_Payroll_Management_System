using Easypay_App.Filters;
using Easypay_App.Interface;
using Easypay_App.Models.DTO;
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
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAuditTrailService _auditTrailService;

        public EmployeeController(IEmployeeService employeeService, IAuditTrailService auditTrailService)
        {
            _employeeService = employeeService;
            _auditTrailService = auditTrailService;
        }

        [HttpPost("add")]
        [Authorize(Roles ="Admin, HR Manager")]
        public async Task<ActionResult> AddEmployee( EmployeeAddRequestDTO employeeDto)
        {
            try
            {
                var result = await _employeeService.AddEmployee(employeeDto);
                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                await _auditTrailService.LogAction(
                    User.Identity.Name,
                    actionId: 1,
                    entityName: "Employee",
                    entityId: result.Id,
                    oldValue: "N/A",
                    newValue: result,
                    ipAddress: ipAddress
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to add Employee");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<ActionResult> UpdateEmployee(int id, [FromBody] EmployeeUpdateRequestDTO dto)
        {
            var oldEmployee = await _employeeService.GetEmployeeById(id);
            var result = await _employeeService.UpdateEmployee(id, dto);
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            await _auditTrailService.LogAction(
                User.Identity.Name,
                actionId: 2, 
                entityName: "Employee",
                entityId: id,
                oldValue: oldEmployee,
                newValue: result,
                ipAddress: ipAddress
            );
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            try
            {
                var oldEmployee = await _employeeService.GetEmployeeById(id);
                var result = await _employeeService.DeleteEmployee(id);
                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                await _auditTrailService.LogAction(
                    User.Identity.Name,
                    actionId: 3,
                    entityName: "Employee",
                    entityId: id,
                    oldValue: oldEmployee,
                    newValue: result,
                    ipAddress: ipAddress
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Delete Employee for Id: {id}");
            }
        }

        [HttpPut("change-userrole")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeUserRole([FromBody] ChangeUserRoleDTO dto)
        {
            var oldEmployee = await _employeeService.GetEmployeeById(dto.EmployeeId);
            var result = await _employeeService.ChangeEmployeeUserRole(dto);
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            await _auditTrailService.LogAction(
                User.Identity.Name,
                actionId: 22,
                entityName: "Employee",
                entityId: dto.EmployeeId,
                oldValue: oldEmployee,
                newValue: result,
                ipAddress: ipAddress
            );
            return Ok(result);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin, HR Manager, Payroll Processor, Manager")]
        public async Task<ActionResult> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployees();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Unable to Get all Employee Details");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, HR Manager,Payroll Processor, Employee, Manager")]
        public async Task<ActionResult> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeById(id);
                return Ok(employee);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Get Employee {id}");
            }
        }

        [HttpPost("search")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<ActionResult> SearchEmployees([FromBody] EmployeeSearchRequestDTO criteria)
        {
            var result = await _employeeService.SearchEmployees(criteria);
            return Ok(result);
        }
    }
}
