namespace Easypay_App.Models.DTO
{
    public class AttendanceResponseDTO
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime WorkDate { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string TotalHours { get; set; }
        public string StatusName { get; set; } = string.Empty;
    }
}
