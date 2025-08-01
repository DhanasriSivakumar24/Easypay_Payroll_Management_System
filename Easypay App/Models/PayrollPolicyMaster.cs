namespace Easypay_App.Models
{
    public class PayrollPolicyMaster
    {
        public int Id { get; set; }
        public string PolicyName { get; set; } = string.Empty;
        public decimal BasicPercent { get; set; }
        public decimal HRAPercent { get; set; }
        public decimal SpecialPercent { get; set; }
        public decimal TravelPercent { get; set; }
        public decimal MedicalPercent { get; set; }
        public decimal EmployeePercent { get; set; }
        public decimal EmployerPercent { get; set; }
        public decimal GratuityPercent { get; set; }
        public string TaxRegime { get; set; } = string.Empty;
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Payroll>? Payrolls { get; set; }

        public PayrollPolicyMaster()
        {
            
        }

        public PayrollPolicyMaster(int id, string policyName, decimal basicPercent, decimal hRAPercent, decimal specialPercent, decimal travelPercent, decimal medicalPercent, decimal employeePercent, decimal employerPercent, decimal gratuityPercent, string taxRegime, DateTime effectiveFrom, DateTime effectiveTo, bool isActive)
        {
            Id = id;
            PolicyName = policyName;
            BasicPercent = basicPercent;
            HRAPercent = hRAPercent;
            SpecialPercent = specialPercent;
            TravelPercent = travelPercent;
            MedicalPercent = medicalPercent;
            EmployeePercent = employeePercent;
            EmployerPercent = employerPercent;
            GratuityPercent = gratuityPercent;
            TaxRegime = taxRegime;
            EffectiveFrom = effectiveFrom;
            EffectiveTo = effectiveTo;
            IsActive = isActive;
        }
    }
}
