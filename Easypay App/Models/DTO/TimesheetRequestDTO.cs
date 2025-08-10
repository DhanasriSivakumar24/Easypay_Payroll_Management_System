namespace Easypay_App.Models.DTO
{
    public class TimesheetRequestDTO
    {
        public int EmployeeId { get; set; }
        public DateTime WorkDate { get; set; }
        public decimal HoursWorked { get; set; }
        public string TaskDescription { get; set; }
        public bool IsBillable { get; set; }
    }
}
