namespace Easypay_App.Models.DTO
{
    public class TimesheetResponseDTO
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public DateTime WorkDate { get; set; }
        public decimal HoursWorked { get; set; }
        public string TaskDescription { get; set; }
        public bool IsBillable { get; set; }
        public string StatusName { get; set; }
    }
}
