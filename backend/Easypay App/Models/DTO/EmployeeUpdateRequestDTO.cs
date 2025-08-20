namespace Easypay_App.Models.DTO
{
    public class EmployeeUpdateRequestDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public DateTime? JoinDate { get; set; }
        public int? StatusId { get; set; }
        public int? DepartmentId { get; set; }
        public int? RoleId { get; set; }
        public int? UserRoleId { get; set; }
        public int? ReportingManagerId { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? PanNumber { get; set; }
        public decimal? Salary { get; set; }
    }
}
