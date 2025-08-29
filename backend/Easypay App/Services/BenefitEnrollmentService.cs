// ✅ FINAL VERSION of BenefitEnrollmentService.cs
using AutoMapper;
using Easypay_App.Exceptions;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;
using Easypay_App.Repositories;

namespace Easypay_App.Services
{
    public class BenefitEnrollmentService : IBenefitEnrollmentService
    {
        private readonly IRepository<int, BenefitMaster> _benefitMasterRepository;
        private readonly IRepository<int, BenefitEnrollment> _benefitEnrollmentRepository;
        private readonly IRepository<int, BenefitStatusMaster> _benefitStatusRepository;
        private readonly IMapper _mapper;

        public BenefitEnrollmentService(
            IRepository<int, BenefitMaster> benefitMasterRepository,
            IRepository<int, BenefitEnrollment> benefitEnrollmentRepository,
            IRepository<int, BenefitStatusMaster> benefitStatusRepository,
            IAuditTrailService auditTrailService,
            IMapper mapper)
        {
            _benefitMasterRepository = benefitMasterRepository;
            _benefitEnrollmentRepository = benefitEnrollmentRepository;
            _benefitStatusRepository = benefitStatusRepository;
            _mapper = mapper;
        }

        public async Task<BenefitEnrollmentAddResponseDTO> EnrollBenefit(BenefitEnrollmentAddRequestDTO request)
        {
            var enrollment = _mapper.Map<BenefitEnrollment>(request);
            enrollment.CreatedAt = DateTime.Now;

            var benefit = await _benefitMasterRepository.GetValueById(request.BenefitId);
            enrollment.EmployeeContribution = benefit.EmployeeContribution;
            enrollment.EmployerContribution = benefit.EmployerContribution;

            await _benefitEnrollmentRepository.AddValue(enrollment);

            var fullEnrollment = await _benefitEnrollmentRepository.GetValueById(enrollment.Id);

            var response = _mapper.Map<BenefitEnrollmentAddResponseDTO>(fullEnrollment);
            await PopulateName(response, fullEnrollment);
            return response;
        }

        public async Task<IEnumerable<BenefitEnrollmentAddResponseDTO>> GetAllBenefit()
        {
            var benefits = await _benefitEnrollmentRepository.GetAllValue();
            if (benefits == null || !benefits.Any())
                throw new NoItemFoundException();

            var response = new List<BenefitEnrollmentAddResponseDTO>();

            foreach (var benefit in benefits)
            {
                var dto = _mapper.Map<BenefitEnrollmentAddResponseDTO>(benefit);
                await PopulateName(dto, benefit);
                response.Add(dto);
            }

            return response;
        }

        public async Task<BenefitEnrollmentAddResponseDTO> GetBenefitById(int id)
        {
            var enrollment = await _benefitEnrollmentRepository.GetValueById(id);
            if (enrollment == null)
                throw new NoItemFoundException();

            var response = _mapper.Map<BenefitEnrollmentAddResponseDTO>(enrollment);
            await PopulateName(response, enrollment);
            return response;
        }

        public async Task<BenefitEnrollmentAddResponseDTO> UpdateBenefit(int id, BenefitEnrollmentAddRequestDTO dto)
        {
            var existing = await _benefitEnrollmentRepository.GetValueById(id);
            if (existing == null)
                throw new NoItemFoundException();

            existing.EmployeeId = dto.EmployeeId;
            existing.BenefitId = dto.BenefitId;
            existing.StartDate = dto.StartDate;
            existing.EndDate = dto.EndDate;
            existing.StatusId = dto.StatusId;
            existing.CreatedAt = DateTime.Now;

            var benefit = await _benefitMasterRepository.GetValueById(dto.BenefitId);
            existing.EmployeeContribution = benefit.EmployeeContribution;
            existing.EmployerContribution = benefit.EmployerContribution;

            await _benefitEnrollmentRepository.UpdateValue(id, existing);

            var response = _mapper.Map<BenefitEnrollmentAddResponseDTO>(existing);
            await PopulateName(response, existing);
            return response;
        }

        public async Task<BenefitEnrollmentAddResponseDTO> DeleteBenefit(int id)
        {
            var enrollment = await _benefitEnrollmentRepository.GetValueById(id);
            if (enrollment == null)
                throw new NoItemFoundException();

            await _benefitEnrollmentRepository.DeleteValue(id);

            var response = _mapper.Map<BenefitEnrollmentAddResponseDTO>(enrollment);
            await PopulateName(response, enrollment);
            return response;
        }

        private async Task PopulateName(BenefitEnrollmentAddResponseDTO response, BenefitEnrollment enrollment)
        {
            var benefit = await _benefitMasterRepository.GetValueById(enrollment.BenefitId);
            var status = await _benefitStatusRepository.GetValueById(enrollment.StatusId);

            response.BenefitName = benefit?.BenefitName ?? "N/A";
            response.StatusName = status?.StatusName ?? "N/A";
            response.EmployeeName = enrollment.Employee != null
                ? $"{enrollment.Employee.FirstName} {enrollment.Employee.LastName}"
                : "N/A";
        }
    }
}
