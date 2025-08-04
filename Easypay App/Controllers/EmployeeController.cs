using Easypay_App.Interface;
using Easypay_App.Models.DTO;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles ="Admin, HR Manager")]
        public ActionResult AddEmployee( EmployeeAddRequestDTO employeeDto)
        {
            try
            {
                var result = _employeeService.AddEmployee(employeeDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to add Employee");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        public ActionResult UpdateEmployee(int id, EmployeeAddRequestDTO employeeDto)
        {
            try
            {
                var result = _employeeService.UpdateEmployee(id, employeeDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Update Employee for Id: {id}");
            }
        }

        [HttpDelete("delete/{id}")]
        public ActionResult DeleteEmployee(int id)
        {
            try
            {
                var result = _employeeService.DeleteEmployee(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Delete Employee for Id: {id}");
            }
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin, HR Manager")]
        public ActionResult GetAllEmployees()
        {
            try
            {
                var employees = _employeeService.GetAllEmployees();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Unable to Get all Employee Details");
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetEmployeeById(int id)
        {
            try
            {
                var employee = _employeeService.GetEmployeeById(id);
                return Ok(employee);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Get Employee {id}");
            }
        }
    }
}
