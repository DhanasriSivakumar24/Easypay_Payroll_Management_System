namespace Easypay_App.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public int LeaveTypeId { get; set; }
        public LeaveTypeMaster? LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StatusId { get; set; }
        public LeaveStatusMaster? Status { get; set; }
        public int ApprovedBy { get; set; }
        public Employee? ApprovedManager { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ActionedAt { get; set; }
        public LeaveRequest() { }

        public LeaveRequest(int id, int employeeId, int leaveTypeId, LeaveTypeMaster? leaveType, DateTime startDate, DateTime endDate, int statusId, LeaveStatusMaster? status, int approvedBy, Employee? approvedManager, DateTime requestedAt, DateTime? actionedAt)
        {
            Id = id;
            EmployeeId = employeeId;
            LeaveTypeId = leaveTypeId;
            LeaveType = leaveType;
            StartDate = startDate;
            EndDate = endDate;
            StatusId = statusId;
            Status = status;
            ApprovedBy = approvedBy;
            ApprovedManager = approvedManager;
            RequestedAt = requestedAt;
            ActionedAt = actionedAt;
        }
    }
}
