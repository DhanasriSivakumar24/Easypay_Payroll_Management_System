namespace Easypay_App.Models
{
    public class Timesheet
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateTime WorkDate { get; set; }
        public decimal HoursWorked { get; set; }
        public string TaskDescription { get; set; } = string.Empty;
        public bool IsBillable { get; set; }
        public int StatusId { get; set; } // Pending, Approved, Rejected
        public TimesheetStatusMaster? Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
