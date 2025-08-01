using Easypay_App.Interfaces;
using Easypay_App.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easypay_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("add")]
        public IActionResult AddEmployee([FromBody] EmployeeAddRequestDTO employeeDto)
        {
            var result = _employeeService.AddEmployee(employeeDto);
            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] EmployeeAddRequestDTO employeeDto)
        {
            var result = _employeeService.UpdateEmployee(id, employeeDto);
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var result = _employeeService.DeleteEmployee(id);
            return Ok(result);
        }

        [HttpGet("all")]
        public IActionResult GetAllEmployees()
        {
            var employees = _employeeService.GetAllEmployees();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            var employee = _employeeService.GetEmployeeById(id);
            return Ok(employee);
        }
    }
}
