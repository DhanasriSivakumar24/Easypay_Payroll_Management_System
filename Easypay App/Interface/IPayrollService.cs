using Easypay_App.Models.DTO;

namespace Easypay_App.Interface
{
    public interface IPayrollService
    {
        public Task<PayrollResponseDTO> GeneratePayroll(PayrollRequestDTO dto);
        public Task<IEnumerable<PayrollResponseDTO>> GetAllPayrolls();
    }
}
