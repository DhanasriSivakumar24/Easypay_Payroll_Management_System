namespace Easypay_App.Models
{
    public class BenefitEnrollment
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public int BenefitId { get; set; }
        public BenefitMaster? Benefit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StatusId { get; set; }
        public BenefitStatusMaster? Status { get; set; }
        public decimal EmployeeContribution { get; set; }
        public decimal EmployerContribution { get; set; }
        public DateTime CreatedAt { get; set; }
        public BenefitEnrollment()
        {
            
        }

        public BenefitEnrollment(int id, int employeeId, Employee? employee, int benefitId, BenefitMaster? benefit, DateTime startDate, DateTime endDate, int statusId, BenefitStatusMaster? status, decimal employeeContribution, decimal employerContribution, DateTime createdAt)
        {
            Id = id;
            EmployeeId = employeeId;
            Employee = employee;
            BenefitId = benefitId;
            Benefit = benefit;
            StartDate = startDate;
            EndDate = endDate;
            StatusId = statusId;
            Status = status;
            EmployeeContribution = employeeContribution;
            EmployerContribution = employerContribution;
            CreatedAt = createdAt;
        }
    }
}
