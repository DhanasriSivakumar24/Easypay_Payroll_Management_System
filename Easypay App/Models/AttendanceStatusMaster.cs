namespace Easypay_App.Models
{
    public class AttendanceStatusMaster
    {
        public int Id { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public ICollection<Attendance>? Attendances { get; set; }
        public AttendanceStatusMaster()
        {
            
        }

        public AttendanceStatusMaster(int id, string statusName)
        {
            Id = id;
            StatusName = statusName;
        }
    }
}
