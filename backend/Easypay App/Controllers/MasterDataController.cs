using Easypay_App.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easypay_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterDataController : ControllerBase
    {
        private readonly PayrollContext _context;

        public MasterDataController(PayrollContext context) 
        {
            _context = context;
        }

        [HttpGet("departments")]
        public ActionResult GetDepartments() => Ok(_context.DepartmentMasters.ToList());

        [HttpGet("roles")]
        public ActionResult GetRoles() => Ok(_context.RoleMasters.ToList());

        [HttpGet("leave-type")]
        public ActionResult GetLeaveType() => Ok(_context.LeaveTypeMasters.ToList());

        [HttpGet("timesheet-type")]
        public ActionResult GetTimesheetType() => Ok(_context.TimesheetStatusMasters.ToList());

        [HttpGet("notification-status")]
        public ActionResult GetNotificationType() => Ok(_context.NotificationStatusMasters.ToList());

        [HttpGet("notification-channel")]
        public ActionResult GetNotificationChannelType() => Ok(_context.NotificationChannelMasters.ToList());

        [HttpGet("user-roles")]
        public ActionResult GetUserRoles() => Ok(_context.UserRoleMasters.ToList());

        [HttpGet("benefit-master")]
        public ActionResult GetBenefits() => Ok(_context.BenefitMasters.ToList());
    }
}
