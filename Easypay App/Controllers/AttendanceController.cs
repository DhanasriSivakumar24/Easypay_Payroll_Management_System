using Easypay_App.Interface;
using Easypay_App.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easypay_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpPost("mark")]
        public async Task<IActionResult> MarkAttendance([FromBody] AttendanceRequestDTO dto)
        {
            var result = await _attendanceService.MarkAttendance(dto);
            return Ok(result);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetAttendanceByEmployee(int employeeId)
        {
            var result = await _attendanceService.GetAttendanceByEmployee(employeeId);
            return Ok(result);
        }
    }
}
