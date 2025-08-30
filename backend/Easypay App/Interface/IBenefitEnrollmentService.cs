using Easypay_App.Models.DTO;

namespace Easypay_App.Interface
{
    public interface IBenefitEnrollmentService
    {
        public Task<BenefitEnrollmentAddResponseDTO> EnrollBenefit(BenefitEnrollmentAddRequestDTO dto);
        public Task<IEnumerable<BenefitEnrollmentAddResponseDTO>> GetAllBenefit();
        public Task<BenefitEnrollmentAddResponseDTO> GetBenefitById(int id);
        public Task<BenefitEnrollmentAddResponseDTO> UpdateBenefit(int id, BenefitEnrollmentAddRequestDTO dto);
        public Task<BenefitEnrollmentAddResponseDTO> DeleteBenefit(int id);
        public Task<IEnumerable<BenefitEnrollmentAddResponseDTO>> GetBenefitsByEmployeeId(int employeeId);
    }
}
