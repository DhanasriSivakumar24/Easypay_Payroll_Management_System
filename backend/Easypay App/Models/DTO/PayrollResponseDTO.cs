namespace Easypay_App.Models.DTO
{
    public class PayrollResponseDTO
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string PolicyName { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;
        public decimal BasicPay { get; set; }
        public decimal Allowances { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetPay { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public DateTime paidDate { get; set; }
    }
}
