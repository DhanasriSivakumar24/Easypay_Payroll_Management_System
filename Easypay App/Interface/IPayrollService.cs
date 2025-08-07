using Easypay_App.Models.DTO;

namespace Easypay_App.Interface
{
    public interface IPayrollService
    {
        public Task<PayrollResponseDTO> GeneratePayroll(PayrollRequestDTO dto);
        public Task<IEnumerable<PayrollResponseDTO>> GetAllPayrolls();
        public Task<PayrollResponseDTO> VerifyPayroll(int payrollId);
        public Task<PayrollResponseDTO> ApprovePayroll(int payrollId);
        public Task<PayrollResponseDTO> MarkPayrollAsPaid(int payrollId, int adminId);
        public Task<IEnumerable<PayrollResponseDTO>> GetPayrollByEmployeeId(int empId);
        public Task<IEnumerable<PayrollResponseDTO>> GenerateComplianceReport(DateTime start, DateTime end);
    }
}
