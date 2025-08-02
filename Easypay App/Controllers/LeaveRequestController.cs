using Easypay_App.Interface;
using Easypay_App.Models.DTO;
using Easypay_App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easypay_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        [HttpGet("all")]
        public ActionResult GetAllRequest()
        {
            var result = _leaveRequestService.GetAllLeaveRequests();
            return Ok(result);
        }
        [HttpPost("submit")]
        public ActionResult SubmitLeave([FromBody] LeaveRequestDTO requestDTO)
        {
            var result = _leaveRequestService.SubmitLeaveRequest(requestDTO);
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public ActionResult Delete(int id)
        {
            var result = _leaveRequestService.DeleteLeaveRequest(id);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult GetLeaveRequestById(int id)
        {
            var result = _leaveRequestService.GetLeaveRequestById(id);
            return Ok(result);
        }

        // Approve leave request
        [HttpPut("approve/{id}")]
        public ActionResult ApproveLeave(int id, int managerId)
        {
            var result = _leaveRequestService.ApproveLeave(id, managerId, true);
            return Ok(result);
        }

        // Reject leave request
        [HttpPut("reject/{id}")]
        public ActionResult RejectLeave(int id, int managerId)
        {
            var result = _leaveRequestService.RejectLeave(id, managerId);
            return Ok(result);
        }
    }
}
