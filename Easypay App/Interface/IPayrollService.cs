using Easypay_App.Models.DTO;

namespace Easypay_App.Interface
{
    public interface IPayrollService
    {
        PayrollResponseDTO GeneratePayroll(PayrollRequestDTO dto);
        IEnumerable<PayrollResponseDTO> GetAllPayrolls();
    }
}
