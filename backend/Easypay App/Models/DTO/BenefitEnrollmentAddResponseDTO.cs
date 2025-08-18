namespace Easypay_App.Models.DTO
{
    public class BenefitEnrollmentAddResponseDTO:BenefitEnrollmentAddRequestDTO
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int BenefitId { get; set; }
        public string? BenefitName { get; set; }
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal EmployeeContribution { get; set; }
        public decimal EmployerContribution { get; set; }
    }
}