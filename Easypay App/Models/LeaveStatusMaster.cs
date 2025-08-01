namespace Easypay_App.Models
{
    public class LeaveStatusMaster
    {
        public int Id { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public ICollection<LeaveRequest>? LeaveRequests { get; set; }

        public LeaveStatusMaster()
        {
            
        }

        public LeaveStatusMaster(int id, string statusName)
        {
            Id = id;
            StatusName = statusName;
        }
    }
}
