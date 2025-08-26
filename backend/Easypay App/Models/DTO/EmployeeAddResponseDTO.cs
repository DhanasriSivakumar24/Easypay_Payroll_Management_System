namespace Easypay_App.Models.DTO
{
    public class EmployeeAddResponseDTO
    {
        public int Id { get; set; }

        // Basic Info
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PanNumber { get; set; } = string.Empty;

        // Extra Personal Info
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }

        // Job Info
        public string DepartmentName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string UserRoleName { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;
        public int? ReportingManager { get; set; }
        public string ReportingManagerName { get; set; } = string.Empty;
        public decimal Salary { get; set; }
    }
}
