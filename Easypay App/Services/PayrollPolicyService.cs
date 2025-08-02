using AutoMapper;
using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Interface;
using Easypay_App.Interfaces;
using Easypay_App.Models;
using Easypay_App.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Easypay_App.Services
{
    public class PayrollPolicyService : IPayrollPolicyService
    {
        private readonly IRepository<int, PayrollPolicyMaster> _payrollPolicyRepository;
        private readonly IMapper _mapper;

        public PayrollPolicyService(IRepository<int,PayrollPolicyMaster> payrollPolicyRepository,
            IMapper mapper)
        {
            _payrollPolicyRepository=payrollPolicyRepository;
            _mapper=mapper;
        }
        public PayrollPolicyAddResponseDTO AddPolicy(PayrollPolicyAddRequestDTO dto)
        {
            var policy = _mapper.Map<PayrollPolicyMaster>(dto);
            _payrollPolicyRepository.AddValue(policy);
            var response = _mapper.Map<PayrollPolicyAddResponseDTO>(policy);
            return response;
        }

        public PayrollPolicyAddResponseDTO DeletePolicy(int id)
        {
            var policy = _payrollPolicyRepository.GetValueById(id);
            if (policy == null)
                throw new NoItemFoundException();
            _payrollPolicyRepository.DeleteValue(id);

            var response = _mapper.Map<PayrollPolicyAddResponseDTO>(policy);
            return response;
        }

        public IEnumerable<PayrollPolicyAddResponseDTO> GetAll()
        {
            var policy = _payrollPolicyRepository.GetAllValue();
            if (policy == null)
                throw new NoItemFoundException();
            var response = policy.Select(_mapper.Map<PayrollPolicyAddResponseDTO>).ToList();
            return response;
        }

        public PayrollPolicyAddResponseDTO GetById(int id)
        {
            var policy = _payrollPolicyRepository.GetValueById(id);
            if (policy == null)
                throw new NoItemFoundException();
            var response =_mapper.Map<PayrollPolicyAddResponseDTO>(policy);
            return response;
        }

        public PayrollPolicyAddResponseDTO UpdatePolicy(int id, PayrollPolicyAddRequestDTO dto)
        {
            var exsistingPolicy = _payrollPolicyRepository.GetValueById(id);
            if (exsistingPolicy == null)
                throw new NoItemFoundException();

            exsistingPolicy.EffectiveFrom = dto.EffectiveFrom;
            exsistingPolicy.EffectiveTo=dto.EffectiveTo;
            exsistingPolicy.PolicyName=dto.PolicyName;
            exsistingPolicy.BasicPercent = dto.BasicPercent;
            exsistingPolicy.HRAPercent = dto.HRAPercent;
            exsistingPolicy.EmployeePercent = dto.EmployeePercent;
            exsistingPolicy.EmployerPercent = dto.EmployerPercent;
            exsistingPolicy.MedicalPercent = dto.MedicalPercent;
            exsistingPolicy.TravelPercent = dto.TravelPercent;
            exsistingPolicy.GratuityPercent = dto.GratuityPercent;
            exsistingPolicy.TaxRegime = dto.TaxRegime;
            exsistingPolicy.IsActive = dto.IsActive;

            _payrollPolicyRepository.UpdateValue(id, exsistingPolicy);
            var response = _mapper.Map<PayrollPolicyAddResponseDTO>(exsistingPolicy);
            return response;
        }
    }
}
