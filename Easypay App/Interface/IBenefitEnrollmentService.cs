using Easypay_App.Models.DTO;

namespace Easypay_App.Interface
{
    public interface IBenefitEnrollmentService
    {
        BenefitEnrollmentAddResponseDTO EnrollBenefit(BenefitEnrollmentAddRequestDTO dto);
        IEnumerable<BenefitEnrollmentAddResponseDTO> GetAllBenefit();
        BenefitEnrollmentAddResponseDTO GetBenefitById(int id);
        BenefitEnrollmentAddResponseDTO UpdateBenefit(int id, BenefitEnrollmentAddRequestDTO dto);
        BenefitEnrollmentAddResponseDTO DeleteBenefit(int id);
    }
}
