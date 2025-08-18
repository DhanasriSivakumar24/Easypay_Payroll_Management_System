namespace Easypay_App.Models.DTO
{
    public class EmployeeComplianceDetailDTO
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal TaxDeducted { get; set; }
        public decimal PFContribution { get; set; }
    }
}
