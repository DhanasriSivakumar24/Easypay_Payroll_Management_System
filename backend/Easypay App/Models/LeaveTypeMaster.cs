namespace Easypay_App.Models
{
    public class LeaveTypeMaster
    {
        public int Id { get; set; }
        public string LeaveTypeName { get; set; } = string.Empty;
        public string LeaveDescription { get; set; } = string.Empty;
        public ICollection<LeaveRequest>? LeaveRequests { get; set; }

        public LeaveTypeMaster()
        {
            
        }
        public LeaveTypeMaster(int id, string leaveTypeName, string description)
        {
            Id = id;
            LeaveTypeName = leaveTypeName;
            LeaveDescription = description;
        }
    }
}
