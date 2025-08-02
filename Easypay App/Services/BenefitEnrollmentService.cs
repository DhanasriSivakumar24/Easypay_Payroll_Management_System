using AutoMapper;
using Easypay_App.Exceptions;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;

namespace Easypay_App.Services
{
    public class BenefitEnrollmentService : IBenefitEnrollmentService
    {
        private readonly IRepository<int, BenefitMaster> _benefitMasterRepository;
        private readonly IRepository<int, BenefitEnrollment> _benefitEnrollmentRepository;
        private readonly IRepository<int, BenefitStatusMaster> _benefitStatusRepository;
        private readonly IMapper _mapper;

        public BenefitEnrollmentService(IRepository<int,BenefitMaster> benefitMasterRepository,
            IRepository<int,BenefitEnrollment> benefitEnrollmentRepository,
            IRepository<int,BenefitStatusMaster> benefitStatusRepository,
            IMapper mapper
            )
        {
            _benefitMasterRepository = benefitMasterRepository;
            _benefitEnrollmentRepository= benefitEnrollmentRepository;
            _benefitStatusRepository= benefitStatusRepository;
            _mapper = mapper;
        }
        public BenefitEnrollmentAddResponseDTO DeleteBenefit(int id)
        {
            var benefits = _benefitEnrollmentRepository.GetValueById(id);
            if (benefits == null)
                throw new NoItemFoundException();

            _benefitEnrollmentRepository.DeleteValue(id);
            var response =_mapper.Map< BenefitEnrollmentAddResponseDTO >(benefits);
            PopulateName(response, benefits);
            return response;
        }

        public BenefitEnrollmentAddResponseDTO EnrollBenefit(BenefitEnrollmentAddRequestDTO dto)
        {
            var enrollment = _mapper.Map<BenefitEnrollment>( dto );
            _benefitEnrollmentRepository.AddValue(enrollment);
            var response = _mapper.Map<BenefitEnrollmentAddResponseDTO>( enrollment );
            PopulateName( response, enrollment );
            return response;
        }

        private void PopulateName(BenefitEnrollmentAddResponseDTO response, BenefitEnrollment enrollment)
        {
            var benefit = _benefitMasterRepository.GetValueById(enrollment.BenefitId);
            var status = _benefitStatusRepository.GetValueById(enrollment.StatusId);

            response.StatusName = status.StatusName;
            response.BenefitName = benefit.BenefitName;
        }

        public IEnumerable<BenefitEnrollmentAddResponseDTO> GetAllBenefit()
        {
            var benefits = _benefitEnrollmentRepository.GetAllValue();
            if (benefits == null || !benefits.Any())
                throw new NoItemFoundException();
            var response = benefits.Select(benefit =>
            {
                var dto = _mapper.Map<BenefitEnrollmentAddResponseDTO>(benefit);
                PopulateName( dto, benefit );
                return dto;
            });
            return response;
        }

        public BenefitEnrollmentAddResponseDTO GetBenefitById(int id)
        {
            var benefits = _benefitEnrollmentRepository.GetValueById(id);
            if (benefits == null)
                throw new NoItemFoundException();

            var response = _mapper.Map<BenefitEnrollmentAddResponseDTO>(benefits);
            PopulateName( response, benefits );
            return response;
        }

        public BenefitEnrollmentAddResponseDTO UpdateBenefit(int id, BenefitEnrollmentAddRequestDTO dto)
        {
            var exsistingBenefit = _benefitEnrollmentRepository.GetValueById(id);
            if (exsistingBenefit == null)
                throw new NoItemFoundException();

            exsistingBenefit.BenefitId = dto.BenefitId;
            exsistingBenefit.EmployeeId=dto.EmployeeId;
            exsistingBenefit.StartDate = dto.StartDate;
            exsistingBenefit.EndDate = dto.EndDate;
            exsistingBenefit.EmployeeContribution = dto.EmployeeContribution;
            exsistingBenefit.EmployerContribution = dto.EmployerContribution;
            exsistingBenefit.StatusId = dto.StatusId;

            _benefitEnrollmentRepository.UpdateValue(id, exsistingBenefit);
            var response =_mapper.Map<BenefitEnrollmentAddResponseDTO>(exsistingBenefit);
            return response;
        }
    }
}
