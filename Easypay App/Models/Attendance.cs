namespace Easypay_App.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateTime WorkDate { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public TimeSpan TotalHours { get; set; }
        public int StatusId { get; set; }
        public AttendanceStatusMaster? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Attendance()
        {
            
        }

        public Attendance(int id, int employeeId, Employee? employee, DateTime workDate, DateTime inTime, DateTime outTime, TimeSpan totalHours, int statusId, AttendanceStatusMaster? status, DateTime createdAt)
        {
            Id = id;
            EmployeeId = employeeId;
            Employee = employee;
            WorkDate = workDate;
            InTime = inTime;
            OutTime = outTime;
            TotalHours = totalHours;
            StatusId = statusId;
            Status = status;
            CreatedAt = createdAt;
        }
    }
}
