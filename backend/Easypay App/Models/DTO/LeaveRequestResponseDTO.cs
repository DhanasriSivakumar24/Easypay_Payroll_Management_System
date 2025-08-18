namespace Easypay_App.Models.DTO
{
    public class LeaveRequestResponseDTO:LeaveRequestDTO
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string LeaveTypeName { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;
        public string? ApprovedManagerName { get; set; } = string.Empty;
        public DateTime RequestedAt { get; set; }
        public DateTime? ActionedAt { get; set; }
    }
}
