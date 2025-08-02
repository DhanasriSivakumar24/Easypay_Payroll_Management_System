namespace Easypay_App.Models.DTO
{
    public class BenefitEnrollmentAddResponseDTO:BenefitEnrollmentAddRequestDTO
    {
        public int Id { get; set; }
        public string? EmployeeName { get; set; }
        public string? BenefitName { get; set; }
        public string? StatusName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}