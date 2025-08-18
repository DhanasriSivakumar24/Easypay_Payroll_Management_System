namespace Easypay_App.Models
{
    public class PayrollDetail
    {
        public int Id { get; set; }
        public int PayrollId { get; set; }
        public Payroll? Payroll { get; set; }
        public string ComponentName { get; set; } = string.Empty;   // Basic Pay, HRA, PF
        public string ComponentType { get; set; } = string.Empty;   // "Earning" / "Deduction"
        public decimal ComponentAmount { get; set; }
        public int TaxId { get; set; }
        public TaxMaster? Tax { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public PayrollDetail()
        {
            
        }

        public PayrollDetail(int id, int payrollId, Payroll? payroll, string componentName, string componentType, decimal componentAmount, int taxId, TaxMaster? tax, string remarks)
        {
            Id = id;
            PayrollId = payrollId;
            Payroll = payroll;
            ComponentName = componentName;
            ComponentType = componentType;
            ComponentAmount = componentAmount;
            TaxId = taxId;
            Tax = tax;
            Remarks = remarks;
        }
    }
}
