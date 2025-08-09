namespace Easypay_App.Models.DTO
{
    public class ComplianceReportDTO
    {
        public int PayrollId { get; set; }
        public DateTime PayrollMonth { get; set; }
        public List<EmployeeComplianceDetailDTO> EmployeeDetails { get; set; }
        public decimal TotalGrossSalary { get; set; }
        public decimal TotalTaxDeducted { get; set; }
        public decimal TotalPFContribution { get; set; }
    }
}
