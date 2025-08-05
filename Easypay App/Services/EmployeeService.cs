using AutoMapper;
using Azure;
using Easypay_App.Exceptions;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;
using EasyPay_App.Repositories;

namespace Easypay_App.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<int, Employee> _employeeRepository;
        private readonly IRepository<int, DepartmentMaster> _departmentRepository;
        private readonly IRepository<int, RoleMaster> _roleRepository;
        private readonly IRepository<int, EmployeeStatusMaster> _statusRepository;
        private readonly IRepository<int, UserRoleMaster> _userRoleRepository;
        private readonly IMapper _mapper;

        public EmployeeService(
            IRepository<int, Employee> employeeRepository,
            IRepository<int, DepartmentMaster> departmentRepository,
            IRepository<int, RoleMaster> roleRepository,
            IRepository<int, EmployeeStatusMaster> statusRepository,
            IRepository<int, UserRoleMaster> userRoleRepository,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _roleRepository = roleRepository;
            _statusRepository = statusRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }

        public async Task<EmployeeAddResponseDTO> AddEmployee(EmployeeAddRequestDTO dto)
        {
            #region 
            //It convert this DTO into the Employee entity, which matches database structure.
            // AutoMapper does this automatically based on mappings you defined.
            #endregion

            var employee = _mapper.Map<Employee>(dto); //mapping dto to employee entity
            employee.UserRoleId = 3;
            await _employeeRepository.AddValue(employee);//Calls repository layer, which in turn uses DbContext to save the employee to the database

            var response = _mapper.Map<EmployeeAddResponseDTO>(employee);
            await PopulateNames(response, employee);
            return response;
        }

        public async Task<EmployeeAddResponseDTO> UpdateEmployee(int id, EmployeeAddRequestDTO dto)
        {
            var existing = await _employeeRepository.GetValueById(id);
            if (existing == null) throw new NoItemFoundException();

            // Map updated values
            existing.FirstName = dto.FirstName;
            existing.LastName = dto.LastName;
            existing.Email = dto.Email;
            existing.PhoneNumber = dto.PhoneNumber;
            existing.DepartmentId = dto.DepartmentId;
            existing.RoleId = dto.RoleId;
            existing.StatusId = dto.StatusId;
            existing.ReportingManagerId = dto.ReportingManagerId;
            existing.Salary = dto.Salary;

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
        }
    }
}
