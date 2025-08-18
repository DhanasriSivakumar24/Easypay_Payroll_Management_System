using Easypay_App.Models.DTO;

namespace Easypay_App.Interface
{
    public interface IPayrollPolicyService
    {
        public Task<PayrollPolicyAddResponseDTO> AddPolicy(PayrollPolicyAddRequestDTO dto);
        public Task<PayrollPolicyAddResponseDTO> UpdatePolicy(int id, PayrollPolicyAddRequestDTO dto);
        public Task<PayrollPolicyAddResponseDTO> DeletePolicy(int id);
        public Task<PayrollPolicyAddResponseDTO> GetById(int id);
        public Task<IEnumerable<PayrollPolicyAddResponseDTO>> GetAll();
    }
}
