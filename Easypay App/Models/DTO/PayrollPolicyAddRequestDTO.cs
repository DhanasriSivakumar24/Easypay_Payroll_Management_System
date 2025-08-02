namespace Easypay_App.Models.DTO
{
    public class PayrollPolicyAddRequestDTO
    {
        public string PolicyName { get; set; }
        public decimal BasicPercent { get; set; }
        public decimal HRAPercent { get; set; }
        public decimal SpecialPercent { get; set; }
        public decimal TravelPercent { get; set; }
        public decimal MedicalPercent { get; set; }
        public decimal EmployeePercent { get; set; }
        public decimal EmployerPercent { get; set; }
        public decimal GratuityPercent { get; set; }
        public string TaxRegime { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public bool IsActive { get; set; }
    }
}
