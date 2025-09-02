using AutoMapper;
using Easypay_App.Exceptions;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;
using Easypay_App.Repositories;

namespace Easypay_App.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<int, Employee> _employeeRepository;
        private readonly IRepository<int, DepartmentMaster> _departmentRepository;
        private readonly IRepository<int, RoleMaster> _roleRepository;
        private readonly IRepository<int, EmployeeStatusMaster> _statusRepository;
        private readonly IRepository<int, UserRoleMaster> _userRoleRepository;
        private readonly IRepository<string, UserAccount> _userAccountRepository;
        private readonly IMapper _mapper;

        public EmployeeService(
            IRepository<int, Employee> employeeRepository,
            IRepository<int, DepartmentMaster> departmentRepository,
            IRepository<int, RoleMaster> roleRepository,
            IRepository<int, EmployeeStatusMaster> statusRepository,
            IRepository<int, UserRoleMaster> userRoleRepository,
            IRepository<string, UserAccount> userAccountRepository,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _roleRepository = roleRepository;
            _statusRepository = statusRepository;
            _userRoleRepository = userRoleRepository;
            _userAccountRepository = userAccountRepository;
            _mapper = mapper;
        }

        public async Task<EmployeeAddResponseDTO> AddEmployee(EmployeeAddRequestDTO dto)
        {
            var employee = _mapper.Map<Employee>(dto);
            await _employeeRepository.AddValue(employee);
            var response = _mapper.Map<EmployeeAddResponseDTO>(employee);
            await PopulateNames(response, employee);
            return response;
        }

        public async Task<EmployeeAddResponseDTO> UpdateEmployee(int id, EmployeeUpdateRequestDTO dto)
        {
            var existing = await _employeeRepository.GetValueById(id);
            if (existing == null) throw new NoItemFoundException();

            if (dto.FirstName != null) existing.FirstName = dto.FirstName;
            if (dto.LastName != null) existing.LastName = dto.LastName;
            if (dto.Email != null) existing.Email = dto.Email;
            if (dto.PhoneNumber != null) existing.PhoneNumber = dto.PhoneNumber;
            if (dto.DepartmentId.HasValue) existing.DepartmentId = dto.DepartmentId.Value;
            if (dto.RoleId.HasValue) existing.RoleId = dto.RoleId.Value;
            if (dto.StatusId.HasValue) existing.StatusId = dto.StatusId.Value;
            if (dto.ReportingManagerId.HasValue) existing.ReportingManagerId = dto.ReportingManagerId.Value;
            if (dto.Salary.HasValue) existing.Salary = dto.Salary.Value;
            if (dto.UserRoleId.HasValue) existing.UserRoleId = dto.UserRoleId.Value;
            if (dto.DateOfBirth.HasValue) existing.DateOfBirth = dto.DateOfBirth.Value;
            if (dto.JoinDate.HasValue) existing.JoinDate = dto.JoinDate.Value;
            if (dto.Address != null) existing.Address = dto.Address;
            if (dto.PanNumber != null) existing.PanNumber = dto.PanNumber;
            if (dto.Gender != null) existing.Gender = dto.Gender;

            await _employeeRepository.UpdateValue(existing.Id, existing);

            var response = _mapper.Map<EmployeeAddResponseDTO>(existing);
            await PopulateNames(response, existing);
            return response;
        }


        public async Task<IEnumerable<EmployeeAddResponseDTO>> GetAllEmployees()
        {
            var employees = await _employeeRepository.GetAllValue();
            if (employees == null || !employees.Any())
                throw new NoItemFoundException();

            var response = new List<EmployeeAddResponseDTO>();

            foreach (var emp in employees)
            {
                var dto = _mapper.Map<EmployeeAddResponseDTO>(emp);
                await PopulateNames(dto, emp);
                response.Add(dto);
            }

            return response;
        }

        public async Task<EmployeeAddResponseDTO> GetEmployeeById(int id)
        {
            var employee = await _employeeRepository.GetValueById(id);
            if (employee == null) throw new NoItemFoundException();

            var response = _mapper.Map<EmployeeAddResponseDTO>(employee);
            await PopulateNames(response, employee);
            return response;
        }

        public async Task<EmployeeAddResponseDTO> DeleteEmployee(int id)
        {
            var employee = await _employeeRepository.GetValueById(id);
            if (employee == null) throw new NoItemFoundException();

            await _employeeRepository.DeleteValue(id);

            var response = _mapper.Map<EmployeeAddResponseDTO>(employee);
            await PopulateNames(response, employee);
            return response;
        }

        public async Task<EmployeeAddResponseDTO> ChangeEmployeeUserRole(ChangeUserRoleDTO dto)
        {
            var employee = await _employeeRepository.GetValueById(dto.EmployeeId);
            if (employee == null) throw new NoItemFoundException();

            var userAccount = employee.UserAccount;
            if (userAccount == null) throw new Exception("UserAccount not found for this employee");

            userAccount.UserRoleId = dto.NewUserRoleId;
            await _userAccountRepository.UpdateValue(userAccount.UserName, userAccount);

            var response = _mapper.Map<EmployeeAddResponseDTO>(employee);
            await PopulateNames(response, employee);
            return response;
        }

        private async Task PopulateNames(EmployeeAddResponseDTO dto, Employee emp)
        {
            var dept = await _departmentRepository.GetValueById(emp.DepartmentId);
            var role = await _roleRepository.GetValueById(emp.RoleId);
            var status = await _statusRepository.GetValueById(emp.StatusId);
            var userRole = await _userRoleRepository.GetValueById(emp.UserRoleId);

            dto.DepartmentName = dept?.DepartmentName ?? "N/A";
            dto.RoleName = role?.RoleName ?? "N/A";
            dto.StatusName = status?.StatusName ?? "N/A";
            dto.UserRoleName = userRole?.UserRoleName ?? "N/A";
            dto.ReportingManager = emp.ReportingManagerId;
            if (emp.ReportingManagerId.HasValue)
            {
                var manager = await _employeeRepository.GetValueById(emp.ReportingManagerId.Value);
                if (manager != null)
                {
                    dto.ReportingManagerName = $"{manager.FirstName} {manager.LastName}";
                }
                else
                {
                    dto.ReportingManagerName = "N/A";
                }
            }
            else
            {
                dto.ReportingManagerName = "N/A";
            }
        }

        public async Task<PaginatedEmployeeResponseDTO> SearchEmployees(EmployeeSearchRequestDTO criteria)
        {
            var employees = await _employeeRepository.GetAllValue();
            employees = employees.Where(e => e.StatusId == 1);
            if (employees.Any() && criteria.PhoneNumber != null)
                employees = await SearchEmployeeByPhoneNumber(employees, criteria.PhoneNumber);
            if (employees.Any() && criteria.FirstName != null)
                employees = await SearchEmployeeByFirstname(employees, criteria.FirstName);
            if (employees.Any() && criteria.LastName != null)
                employees = await SearchEmployeeByLastname(employees, criteria.LastName);
            if (employees.Any() && criteria.Email != null)
                employees = await SearchEmployeeByEmail(employees, criteria.Email);
            if (employees.Any() && criteria.Departments != null)
                employees = await SearchEmployeeByDepartment(employees, criteria.Departments);
            if (employees.Any() && criteria.RoleId != null)
                employees = await SearchEmployeeByRole(employees, criteria.RoleId);
            if (employees.Any() && criteria.UserRoleId != null)
                employees = await SearchEmployeeByUserRole(employees, criteria.UserRoleId);
            if (employees.Any() && criteria.ReportingManagerId != null)
                employees = await SearchEmployeeByReportingManagerId(employees, criteria.ReportingManagerId);
            if (employees.Any() && criteria.SalaryRange != null)
                employees = await SearchEmployeeByReportingSalaryRange(employees, criteria.SalaryRange);
            if (employees.Any() && criteria.Sort != null)
                employees = await SearchEmployeeByReportingSort(employees, criteria.Sort);
            if (employees.Any())
            {
                var totalRecords = employees.Count();
                var result = await PopulateEmployeeDetails(employees);

                result = result.Skip((criteria.PageNumber - 1) * criteria.PageSize).Take(criteria.PageSize).ToList();

                return new PaginatedEmployeeResponseDTO
                {
                    Employees = result.ToList(),
                    TotalNumberOfRecords = totalRecords,
                    PageNumber = criteria.PageNumber
                };
            }
            throw new Exception("No search result Found");
        }

        private async Task<List<EmployeeAddResponseDTO>> PopulateEmployeeDetails(IEnumerable<Employee> employees)
        {
            var result = new List<EmployeeAddResponseDTO>();

            foreach (var emp in employees)
            {
                var dto = _mapper.Map<EmployeeAddResponseDTO>(emp);
                await PopulateNames(dto, emp);
                result.Add(dto);
            }

            return result;
        }

        private async Task<IEnumerable<Employee>> SearchEmployeeByReportingSort(IEnumerable<Employee> employees, int sort)
        {
            switch (sort)
            {
                case -3:
                    employees = employees.OrderByDescending(e => e.Id); break;
                case -2:
                    employees = employees.OrderByDescending(e => e.FirstName); break;
                case -1:
                    employees = employees.OrderByDescending(e => e.DepartmentId); break;
                case 1:
                    employees = employees.OrderBy(e => e.Id); break;
                case 2:
                    employees = employees.OrderBy(e => e.FirstName); break;
                case 3:
                    employees = employees.OrderBy(e => e.DepartmentId); break;
                default:
                    break;
            }
            return employees;
        }

        private async Task<IEnumerable<Employee>> SearchEmployeeByReportingSalaryRange(IEnumerable<Employee> employees, SearchRange<decimal> salaryRange)
        {
            var result = employees.Where(emp => emp.Salary >= salaryRange.MinValue && emp.Salary <= salaryRange.MaxValue).ToList();
            return result;
        }

        private async Task<IEnumerable<Employee>> SearchEmployeeByReportingManagerId(IEnumerable<Employee> employees, List<int>? reportingManagerId)
        {
            var result = new List<Employee>();
            foreach (var manager in reportingManagerId)
                result.AddRange(employees.Where(e => e.ReportingManagerId == manager).ToList());
            return result;
        }

        private async Task<IEnumerable<Employee>> SearchEmployeeByUserRole(IEnumerable<Employee> employees, List<int>? userRoleId)
        {
            var result = new List<Employee>();
            foreach (var role in userRoleId)
                result.AddRange(employees.Where(e => e.UserRoleId == role).ToList());
            return result;
        }

        private async Task<IEnumerable<Employee>> SearchEmployeeByRole(IEnumerable<Employee> employees, List<int>? roleId)
        {
            var result = new List<Employee>();
            foreach (var role in roleId)
                result.AddRange(employees.Where(e => e.RoleId == role).ToList());
            return result;
        }

        private async Task<IEnumerable<Employee>> SearchEmployeeByDepartment(IEnumerable<Employee> employees, List<int>? departments)
        {
            var result = new List<Employee>();
            foreach (var dep in departments)
                result.AddRange(employees.Where(e => e.DepartmentId == dep).ToList());
            return result;
        }

        private async Task<IEnumerable<Employee>> SearchEmployeeByEmail(IEnumerable<Employee> employees, string email)
        {
            email = email.ToLower();
            var result = employees.Where(emp => emp.Email.ToLower().Contains(email)).ToList();
            return result;
        }

        private async Task<IEnumerable<Employee>> SearchEmployeeByLastname(IEnumerable<Employee> employees, string? lastName)
        {
            lastName = lastName.ToLower();
            var result = employees.Where(emp => emp.LastName.ToLower().Contains(lastName)).ToList();
            return result;
        }

        private async Task<IEnumerable<Employee>> SearchEmployeeByFirstname(IEnumerable<Employee> employees, string firstName)
        {
            firstName = firstName.ToLower();
            var result = employees.Where(emp => emp.FirstName.ToLower().Contains(firstName)).ToList();
            return result;
        }

        private async Task<IEnumerable<Employee>> SearchEmployeeByPhoneNumber(IEnumerable<Employee> employees, string phoneNumber)
        {
            var result = employees.Where(emp => emp.PhoneNumber == phoneNumber).ToList();
            return result;
        }
    }
}