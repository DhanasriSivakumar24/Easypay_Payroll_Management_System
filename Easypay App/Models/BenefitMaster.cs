namespace Easypay_App.Models
{
    public class BenefitMaster
    {
        public int Id { get; set; }
        public string BenefitName { get; set; } = string.Empty;
        public string BenefitDescription { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public decimal EmployeeContribution { get; set; }
        public decimal EmployerContribution { get; set; }
        public bool IsTaxable { get; set; }
        public bool IsEmployerContribution { get; set; }
        public bool IsActive { get; set; }
        public ICollection<BenefitEnrollment>? BenefitEnrollments { get; set; }//to know all BenefitEnrollments that share a particular Benefits

        public BenefitMaster()
        {
            
        }
        public BenefitMaster(int id, string benefitName, string benefitDescription, DateTime createdDate, DateTime modifiedDate, decimal employeeContribution, decimal employerContribution, bool isTaxable, bool isEmployerContribution, bool isActive, ICollection<BenefitEnrollment>? benefitEnrollments)
        {
            Id = id;
            BenefitName = benefitName;
            BenefitDescription = benefitDescription;
            CreatedDate = createdDate;
            ModifiedDate = modifiedDate;
            EmployeeContribution = employeeContribution;
            EmployerContribution = employerContribution;
            IsTaxable = isTaxable;
            IsEmployerContribution = isEmployerContribution;
            IsActive = isActive;
            BenefitEnrollments = benefitEnrollments;
        }
    }
}
