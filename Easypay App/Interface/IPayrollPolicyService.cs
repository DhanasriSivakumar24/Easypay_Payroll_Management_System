using Easypay_App.Models.DTO;

namespace Easypay_App.Interface
{
    public interface IPayrollPolicyService
    {
        PayrollPolicyAddResponseDTO AddPolicy(PayrollPolicyAddRequestDTO dto);
        PayrollPolicyAddResponseDTO UpdatePolicy(int id, PayrollPolicyAddRequestDTO dto);
        PayrollPolicyAddResponseDTO DeletePolicy(int id);
        PayrollPolicyAddResponseDTO GetById(int id);
        IEnumerable<PayrollPolicyAddResponseDTO> GetAll();
    }
}
