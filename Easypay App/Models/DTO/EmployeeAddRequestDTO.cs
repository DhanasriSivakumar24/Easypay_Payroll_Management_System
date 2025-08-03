namespace Easypay_App.Models.DTO
{
    public class EmployeeAddRequestDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        public int StatusId { get; set; }
        public int DepartmentId { get; set; }
        public int RoleId { get; set; }
        public int? ReportingManagerId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PanNumber { get; set; } = string.Empty;
        public decimal Salary { get; set; }
    }
}
