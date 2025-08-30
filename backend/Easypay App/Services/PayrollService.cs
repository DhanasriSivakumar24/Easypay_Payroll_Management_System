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

        #region GetPayrollById
        public async Task<PayrollResponseDTO> GetPayrollById(int payrollId)
        {
            var payroll = await _payrollRepository.GetValueById(payrollId);
            if (payroll == null)
                throw new NoItemFoundException();

            return await MapPayrollToDTO(payroll);
        }
        #endregion

        #region ApprovePayroll
        public async Task<PayrollResponseDTO> ApprovePayroll(int payrollId)
        {
            var payroll = await _payrollRepository.GetValueById(payrollId);
            if (payroll == null)
                throw new NoItemFoundException();

            payroll.StatusId = 3; // Approved
            payroll.ApprovedDate = DateTime.Now;
            await _payrollRepository.UpdateValue(payroll.Id, payroll);

            return await MapPayrollToDTO(payroll);
        }
        #endregion

        #region GetApprovedPayrolls
        public async Task<IEnumerable<PayrollResponseDTO>> GetApprovedPayrolls(DateTime start, DateTime end)
        {
            var all = await _payrollRepository.GetAllValue();
            var filtered = all.Where(p =>
                p.PeriodStart <= end &&
                p.PeriodEnd >= start &&
                (p.StatusId == 3 || p.StatusId == 5)); // Approved and Paid

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

            // Check full-month period
            if (dto.PeriodStart.Day != 1 || dto.PeriodEnd.Day != DateTime.DaysInMonth(dto.PeriodEnd.Year, dto.PeriodEnd.Month))
                throw new InvalidOperationException("Payroll must cover a full month (1st to last day).");

            var existingPayrolls = await _payrollRepository.GetAllValue();
            bool exists = existingPayrolls.Any(p =>
                p.EmployeeId == dto.EmployeeId &&
                p.PeriodStart.Month == dto.PeriodStart.Month &&
                p.PeriodStart.Year == dto.PeriodStart.Year);

            if (exists)
                throw new InvalidOperationException("Payroll already generated for this employee for the selected month.");

            if (policy.BasicPercent + policy.HRAPercent + policy.SpecialPercent +
                policy.TravelPercent + policy.MedicalPercent > 100)
                throw new InvalidOperationException("Invalid payroll policy: total percentages exceed 100%.");

            decimal gross = employee.Salary;

            var timesheets = (await _timesheetRepository.GetAllValue())
                .Where(t => t.EmployeeId == dto.EmployeeId &&
                            t.WorkDate >= dto.PeriodStart &&
                            t.WorkDate <= dto.PeriodEnd)
                .ToList();

            if (!timesheets.Any())
                throw new InvalidOperationException("No timesheets found for this employee in the selected period.");

            decimal totalHoursWorked = timesheets.Sum(t => t.HoursWorked);
            int totalWorkingDays = (dto.PeriodEnd - dto.PeriodStart).Days + 1;
            decimal expectedHours = totalWorkingDays * 8;

            decimal timesheetPenalty = 0;
            if (totalHoursWorked < expectedHours)
            {
                decimal hourlyRate = gross / (expectedHours > 0 ? expectedHours : 1);
                timesheetPenalty = (expectedHours - totalHoursWorked) * hourlyRate;
            }

            decimal basic = (gross * policy.BasicPercent) / 100;
            decimal hra = (gross * policy.HRAPercent) / 100;
            decimal special = (gross * policy.SpecialPercent) / 100;
            decimal travel = (gross * policy.TravelPercent) / 100;
            decimal medical = (gross * policy.MedicalPercent) / 100;
            decimal allowances = hra + special + travel + medical;

            // Deductions
            decimal employeePf = (gross * policy.EmployeePercent) / 100;
            decimal deductions = employeePf + timesheetPenalty;

            decimal netPay = gross + allowances - deductions;
            if (netPay < 0) netPay = 0;

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
                StatusId = 1, // Pending
                GeneratedDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                PaidBy = null,
                PaidDate = null
            };

            await _payrollRepository.AddValue(payroll);

            var response = _mapper.Map<PayrollResponseDTO>(payroll);
            response.EmployeeName = $"{employee.FirstName} {employee.LastName}";
            response.PolicyName = policy.PolicyName;
            response.StatusName = (await _statusRepository.GetValueById(payroll.StatusId))?.StatusName ?? "Pending";

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

            payroll.StatusId = 5; // Paid
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
                .Where(p =>
                p.PeriodStart <= end &&
                p.PeriodEnd >= start &&
                (p.StatusId == 3 || p.StatusId == 5)) // Approved and paid
                .ToList();

            var employeeDetails = new List<EmployeeComplianceDetailDTO>();

            decimal totalGrossSalary = 0;
            decimal totalPFContribution = 0;

            foreach (var payroll in filtered)
            {
                var emp = await _employeeRepository.GetValueById(payroll.EmployeeId);
                var policy = await _policyRepository.GetValueById(payroll.PolicyId);

                decimal grossSalary = payroll.BasicPay + payroll.Allowances; // we are using the payroll data
                decimal deductions = payroll.Deductions;
                decimal netPay = payroll.NetPay;
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
                    PFContribution = pfTotal
                });
            }

            return new ComplianceReportDTO
            {
                PayrollId = 0, // NA for a period report
                PayrollMonth = start, // adjusted acc. to reporting period start
                EmployeeDetails = employeeDetails,
                TotalGrossSalary = totalGrossSalary,
                TotalPFContribution = totalPFContribution
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
