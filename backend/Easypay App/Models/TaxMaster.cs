namespace Easypay_App.Models
{
    public class TaxMaster
    {
        public int Id { get; set; }
        public float MinIncome { get; set; }
        public float MaxIncome { get; set; }
        public decimal TaxRate { get; set; }
        public float TaxAmount { get; set; }
        public ICollection<PayrollDetail>? PayrollDetails { get; set; }

        public TaxMaster()
        {
            
        }
        public TaxMaster(int id, float minIncome, float maxIncome, decimal taxRate, float taxAmount)
        {
            Id = id;
            MinIncome = minIncome;
            MaxIncome = maxIncome;
            TaxRate = taxRate;
            TaxAmount = taxAmount;
        }
    }
}
