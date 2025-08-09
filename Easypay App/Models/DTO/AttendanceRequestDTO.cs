namespace Easypay_App.Models.DTO
{
    public class AttendanceRequestDTO
    {
        public int EmployeeId { get; set; }
        public DateTime WorkDate { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public int StatusId { get; set; }
    }
}
