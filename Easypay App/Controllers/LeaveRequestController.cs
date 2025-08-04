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
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public ActionResult GetAllRequest()
        {
            try
            {
                var result = _leaveRequestService.GetAllLeaveRequests();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Unable to Get all Leave Request Details");
            }
        }

        [HttpPost("submit")]
        public ActionResult SubmitLeave([FromBody] LeaveRequestDTO requestDTO)
        {
            try
            {
                var result = _leaveRequestService.SubmitLeaveRequest(requestDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Unable to Submit Leave Request ");
            }
        }

        [HttpDelete("delete/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var result = _leaveRequestService.DeleteLeaveRequest(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Delete Leave Request for Id: {id}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetLeaveRequestById(int id)
        {
            try
            {
                var result = _leaveRequestService.GetLeaveRequestById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Get Leave Request for Id: {id}");
            }
        }

        // Approve leave request
        [HttpPut("approve/{id}")]
        [Authorize(Roles ="Admin")]
        public ActionResult ApproveLeave(int id, int managerId)
        {
            try
            {
                var result = _leaveRequestService.ApproveLeave(id, managerId, true);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Approve Leave Request for Id: {id}");
            }
        }

        // Reject leave request
        [HttpPut("reject/{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult RejectLeave(int id, int managerId)
        {
            try
            {
                var result = _leaveRequestService.RejectLeave(id, managerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Unable to Reject Leave Request for Id: {id}");
            }
        }
    }
}
