namespace Easypay_App.Models.DTO
{
    public class EmployeeAddResponseDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;
        public int? ReportingManager { get; set; }
        public string ReportingManagerName { get; internal set; }
    }
}
