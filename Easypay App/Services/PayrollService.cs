using AutoMapper;
using Easypay_App.Exceptions;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;
using Easypay_App.Repositories;
using EasyPay_App.Repositories;

namespace Easypay_App.Services
{
    public class PayrollService : IPayrollService
    {
        private readonly IRepository<int, Employee> _employeeRepository;
        private readonly IRepository<int, PayrollPolicyMaster> _policyRepository;
        private readonly IRepository<int, PayrollStatusMaster> _statusRepository;
        private readonly IRepository<int, Payroll> _payrollRepository;
        private readonly IRepository<int, Timesheet> _timesheetRepository;
        private readonly IMapper _mapper;

        public PayrollService(
            IRepository<int, Employee> employeeRepository,
            IRepository<int, PayrollPolicyMaster> policyRepository,
            IRepository<int, PayrollStatusMaster> statusRepository,
            IRepository<int, Payroll> payrollRepository,
            IRepository<int, Timesheet> timesheetRepository,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _policyRepository = policyRepository;
            _statusRepository = statusRepository;
            _payrollRepository = payrollRepository;
            _timesheetRepository = timesheetRepository;
            _mapper = mapper;
        }

        #region MapPayrollToDTO
        private async Task<PayrollResponseDTO> MapPayrollToDTO(Payroll payroll)
        {
            var dto = _mapper.Map<PayrollResponseDTO>(payroll);
            var emp = await _employeeRepository.GetValueById(payroll.EmployeeId);
            var policy = await _policyRepository.GetValueById(payroll.PolicyId);
            var status = await _statusRepository.GetValueById(payroll.StatusId);

            dto.EmployeeName = emp != null ? $"{emp.FirstName} {emp.LastName}" : "Unknown";
            dto.PolicyName = policy?.PolicyName ?? "Unknown";
            dto.StatusName = status?.StatusName ?? "Unknown";

            return dto;
        }
        #endregion

        #region ApprovePayroll
        public async Task<PayrollResponseDTO> ApprovePayroll(int payrollId)
        {
            var payroll = await _payrollRepository.GetValueById(payrollId);
            if (payroll == null)
                throw new NoItemFoundException();

            payroll.StatusId = 3; // Approved
            await _payrollRepository.UpdateValue(payroll.Id, payroll);

            return await MapPayrollToDTO(payroll);
        }
        #endregion

        #region GetApprovedPayrolls
        public async Task<IEnumerable<PayrollResponseDTO>> GetApprovedPayrolls(DateTime start, DateTime end)
        {
            var all = await _payrollRepository.GetAllValue();
            var filtered = all.Where(p =>
                p.PeriodStart >= start &&
                p.PeriodEnd <= end &&
                p.StatusId == 3); // Only Approved

            var responseList = new List<PayrollResponseDTO>();
            foreach (var p in filtered)
            {
                responseList.Add(await MapPayrollToDTO(p));
            }

            return responseList;
        }
        #endregion

        #region GeneratePayroll

        public async Task<PayrollResponseDTO> GeneratePayroll(PayrollRequestDTO dto)
        {
            var employee = await _employeeRepository.GetValueById(dto.EmployeeId);
            var policy = await _policyRepository.GetValueById(dto.PolicyId);
            if (employee == null || policy == null)
                throw new NoItemFoundException();

            decimal gross = employee.Salary;

            // ===== Timesheet-based calculations =====
            var timesheets = (await _timesheetRepository.GetAllValue())
                .Where(t => t.EmployeeId == dto.EmployeeId &&
                            t.WorkDate >= dto.PeriodStart &&
                            t.WorkDate <= dto.PeriodEnd)
                .ToList();

            decimal totalHoursWorked = timesheets.Sum(t => t.HoursWorked);

            // Assume 8 hours per working day in the period
            int totalWorkingDays = (dto.PeriodEnd - dto.PeriodStart).Days + 1;
            decimal expectedHours = totalWorkingDays * 8;

            decimal timesheetPenalty = 0;
            if (totalHoursWorked < expectedHours)
            {
                decimal hourlyRate = gross / (expectedHours > 0 ? expectedHours : 1);
                timesheetPenalty = (expectedHours - totalHoursWorked) * hourlyRate;
            }

            // ===== Policy-based calculations =====
            decimal basic = (gross * policy.BasicPercent) / 100;
            decimal hra = (gross * policy.HRAPercent) / 100;
            decimal special = (gross * policy.SpecialPercent) / 100;
            decimal travel = (gross * policy.TravelPercent) / 100;
            decimal medical = (gross * policy.MedicalPercent) / 100;

            decimal allowances = hra + special + travel + medical;
            decimal employeePf = (gross * policy.EmployeePercent) / 100;
            decimal gratuity = (gross * policy.GratuityPercent) / 100;

            decimal deductions = employeePf + gratuity + timesheetPenalty;
            decimal netPay = gross - deductions;

            // ===== Save payroll =====
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
                StatusId = 1,
                GeneratedDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                PaidBy = 1,
                PaidDate = DateTime.Now
            };

            await _payrollRepository.AddValue(payroll);

            var response = _mapper.Map<PayrollResponseDTO>(payroll);
            response.EmployeeName = $"{employee.FirstName} {employee.LastName}";
            response.PolicyName = policy.PolicyName;
            response.StatusName = (await _statusRepository.GetValueById(payroll.StatusId)).StatusName;
            return response;
        }
        #endregion

        #region GetAllPayrolls
        public async Task<IEnumerable<PayrollResponseDTO>> GetAllPayrolls()
        {
            var all = await _payrollRepository.GetAllValue();
            var responseList = new List<PayrollResponseDTO>();

            foreach (var p in all)
            {
                responseList.Add(await MapPayrollToDTO(p));
            }

            return responseList;
        }
        #endregion

        #region GetPayrollByEmployeeId
        public async Task<IEnumerable<PayrollResponseDTO>> GetPayrollByEmployeeId(int empId)
        {
            var allPayrolls = await _payrollRepository.GetAllValue();
            var filtered = allPayrolls.Where(p => p.EmployeeId == empId);

            var responseList = new List<PayrollResponseDTO>();
            foreach (var p in filtered)
            {
                responseList.Add(await MapPayrollToDTO(p));
            }

            return responseList;
        }
        #endregion

        #region MarkPayrollAsPaid
        public async Task<PayrollResponseDTO> MarkPayrollAsPaid(int payrollId, int adminId)
        {
            var payroll = await _payrollRepository.GetValueById(payrollId);
            if (payroll == null)
                throw new NoItemFoundException();

            payroll.StatusId = 3; // Approved
            payroll.PaidBy = adminId;
            payroll.PaidDate = DateTime.Now;

            await _payrollRepository.UpdateValue(payroll.Id, payroll);

            return await MapPayrollToDTO(payroll);
        }
        #endregion

        #region GenerateComplianceReport
        public async Task<ComplianceReportDTO> GenerateComplianceReport(DateTime start, DateTime end)
        {
            var allPayrolls = await _payrollRepository.GetAllValue();
            var filtered = allPayrolls
                .Where(p => p.PeriodStart >= start && p.PeriodEnd <= end && p.StatusId == 3) // Approved only
                .ToList();

            var employeeDetails = new List<EmployeeComplianceDetailDTO>();

            decimal totalGrossSalary = 0;
            decimal totalPFContribution = 0;
            decimal totalTaxDeducted = 0; // Placeholder until tax logic is added

            foreach (var payroll in filtered)
            {
                var emp = await _employeeRepository.GetValueById(payroll.EmployeeId);
                var policy = await _policyRepository.GetValueById(payroll.PolicyId);

                decimal grossSalary = emp?.Salary ?? 0;
                decimal employeePF = (grossSalary * (policy?.EmployeePercent ?? 0)) / 100;
                decimal employerPF = (grossSalary * (policy?.EmployerPercent ?? 0)) / 100;
                decimal pfTotal = employeePF + employerPF;

                totalGrossSalary += grossSalary;
                totalPFContribution += pfTotal;

                employeeDetails.Add(new EmployeeComplianceDetailDTO
                {
                    EmployeeId = payroll.EmployeeId,
                    EmployeeName = emp != null ? $"{emp.FirstName} {emp.LastName}" : "Unknown",
                    GrossSalary = grossSalary,
                    PFContribution = pfTotal,
                    TaxDeducted = 0 // Placeholder
                });
            }

            return new ComplianceReportDTO
            {
                PayrollId = 0, // Not applicable for a period report
                PayrollMonth = start, // Can be adjusted to reporting period start
                EmployeeDetails = employeeDetails,
                TotalGrossSalary = totalGrossSalary,
                TotalPFContribution = totalPFContribution,
                TotalTaxDeducted = totalTaxDeducted
            };
        }
        #endregion

        #region VerifyPayroll
        public async Task<PayrollResponseDTO> VerifyPayroll(int payrollId)
        {
            var payroll = await _payrollRepository.GetValueById(payrollId);
            if (payroll == null)
                throw new NoItemFoundException();

            payroll.StatusId = 2; // Processed
            payroll.PaidDate = DateTime.Now;
            await _payrollRepository.UpdateValue(payroll.Id, payroll);

            return await MapPayrollToDTO(payroll);
        }
        #endregion

    }
}
