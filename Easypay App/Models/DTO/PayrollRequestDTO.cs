namespace Easypay_App.Models.DTO
{
    public class PayrollRequestDTO
    {
        public int EmployeeId { get; set; }
        public int PolicyId { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}
