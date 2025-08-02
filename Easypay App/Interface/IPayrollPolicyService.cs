using Easypay_App.Models.DTO;

namespace Easypay_App.Interface
{
    public interface IPayrollPolicyService
    {
        PayrollPolicyResponseDTO AddPolicy(PayrollPolicyRequestDTO dto);
        PayrollPolicyResponseDTO UpdatePolicy(int id, PayrollPolicyRequestDTO dto);
        PayrollPolicyResponseDTO DeletePolicy(int id);
        PayrollPolicyResponseDTO GetById(int id);
        IEnumerable<PayrollPolicyResponseDTO> GetAll();
    }
}
