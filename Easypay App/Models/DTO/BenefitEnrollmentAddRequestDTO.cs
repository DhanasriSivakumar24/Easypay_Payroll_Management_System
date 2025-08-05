namespace Easypay_App.Models.DTO
{
    public class BenefitEnrollmentAddRequestDTO
    {
        public int EmployeeId { get; set; }
        public int BenefitId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StatusId { get; set; }
    }
}
