using AutoMapper;
using Easypay_App.Exceptions;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;
using Easypay_App.Repositories;
using EasyPay_App.Repositories;

namespace Easypay_App.Services
{
    public class PayrollService:IPayrollService
    {
        private readonly IRepository<int, Employee> _employeeRepository;
        private readonly IRepository<int, PayrollPolicyMaster> _policyRepository;
        private readonly IRepository<int, PayrollStatusMaster> _statusRepository;
        private readonly IRepository<int, Payroll> _payrollRepository;
        private readonly IMapper _mapper;

        public PayrollService(IRepository<int, Employee> employeeRepository,
                              IRepository<int, PayrollPolicyMaster> policyRepository,
                              IRepository<int, PayrollStatusMaster> statusRepository,
                              IRepository<int, Payroll> payrollRepository,
                              IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _policyRepository = policyRepository;
            _statusRepository = statusRepository;
            _payrollRepository = payrollRepository;
            _mapper = mapper;
        }

        public PayrollResponseDTO GeneratePayroll(PayrollRequestDTO dto)
        {
            var employee = _employeeRepository.GetValueById(dto.EmployeeId);
            var policy = _policyRepository.GetValueById(dto.PolicyId);
            if (employee == null || policy == null)
                throw new NoItemFoundException();

            decimal gross = employee.Salary;

            // Calculate based on policy percentages
            decimal basic = (gross * policy.BasicPercent) / 100;
            decimal hra = (gross * policy.HRAPercent) / 100;
            decimal special = (gross * policy.SpecialPercent) / 100;
            decimal travel = (gross * policy.TravelPercent) / 100;
            decimal medical = (gross * policy.MedicalPercent) / 100;

            decimal allowances = hra + special + travel + medical;
            decimal employeePf = (gross * policy.EmployeePercent) / 100;
            decimal employerPf = (gross * policy.EmployerPercent) / 100;
            decimal gratuity = (gross * policy.GratuityPercent) / 100;
            decimal deductions = employeePf + gratuity;
            decimal netPay = gross - deductions;

            var payroll = new Payroll
            {
                EmployeeId = dto.EmployeeId,
                PolicyId = dto.PolicyId,
                PeriodStart = dto.PeriodStart,
                PeriodEnd = dto.PeriodEnd,
                BasicPay = basic,
                Allowances = allowances,
                Deductions = deductions,
                NetPay = netPay,
                StatusId = 1, // e.g. Pending
                GeneratedDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                PaidBy = 1, // Admin
                PaidDate = DateTime.Now
            };

            _payrollRepository.AddValue(payroll);

            var response = _mapper.Map<PayrollResponseDTO>(payroll);
            response.EmployeeName = $"{employee.FirstName} {employee.LastName}";
            response.PolicyName = policy.PolicyName;
            response.StatusName = _statusRepository.GetValueById(payroll.StatusId).StatusName;
            return response;
        }

        public IEnumerable<PayrollResponseDTO> GetAllPayrolls()
        {
            var all = _payrollRepository.GetAllValue();
            return all.Select(p =>
            {
                var dto = _mapper.Map<PayrollResponseDTO>(p);
                var emp = _employeeRepository.GetValueById(p.EmployeeId);
                var policy = _policyRepository.GetValueById(p.PolicyId);
                var status = _statusRepository.GetValueById(p.StatusId);

                dto.EmployeeName = $"{emp.FirstName} {emp.LastName}";
                dto.PolicyName = policy.PolicyName;
                dto.StatusName = status.StatusName;

                return dto;
            });
        }
    }
}
