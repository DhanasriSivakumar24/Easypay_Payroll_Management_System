namespace Easypay_App.Models
{
    public class PayrollStatusMaster
    {
        public int Id { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public ICollection<Payroll>? Payrolls { get; set; }

        public PayrollStatusMaster()
        {
            
        }

        public PayrollStatusMaster(int id, string statusName)
        {
            Id = id;
            StatusName = statusName;
        }
    }
}
