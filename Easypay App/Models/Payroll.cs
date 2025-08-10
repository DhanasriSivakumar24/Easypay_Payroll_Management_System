namespace Easypay_App.Models
{
    public class Payroll
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public int PolicyId { get; set; }
        public PayrollPolicyMaster? Policy { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public decimal BasicPay { get; set; }
        public decimal Allowances { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetPay { get; set; }
        public int StatusId { get; set; }
        public PayrollStatusMaster? Status { get; set; }
        public DateTime GeneratedDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int? ApprovedBy { get; set; }
        public Employee? ApprovedById { get; set; }
        public DateTime? PaidDate { get; set; }
        public int? PaidBy { get; set; }
        public UserRoleMaster? PaidById { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<PayrollDetail>? PayrollDetails { get; set; }

        public Payroll()
        {
            
        }
        public Payroll(int id, int employeeId, Employee? employee, int policyId, PayrollPolicyMaster? policy, DateTime periodStart, DateTime periodEnd, decimal basicPay, decimal allowances, decimal deductions, decimal netPay, int statusId, PayrollStatusMaster? status, DateTime generatedDate, DateTime approvedDate, int? approvedBy, Employee? approvedById, DateTime paidDate, int paidBy, UserRoleMaster? paidById, DateTime createdAt)
        {
            Id = id;
            EmployeeId = employeeId;
            Employee = employee;
            PolicyId = policyId;
            Policy = policy;
            PeriodStart = periodStart;
            PeriodEnd = periodEnd;
            BasicPay = basicPay;
            Allowances = allowances;
            Deductions = deductions;
            NetPay = netPay;
            StatusId = statusId;
            Status = status;
            GeneratedDate = generatedDate;
            ApprovedDate = approvedDate;
            ApprovedBy = approvedBy;
            ApprovedById = approvedById;
            PaidDate = paidDate;
            PaidBy = paidBy;
            PaidById = paidById;
            CreatedAt = createdAt;
        }
    }
}
