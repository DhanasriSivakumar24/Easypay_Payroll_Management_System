using AutoMapper;
using Easypay_App.Exceptions;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;

namespace Easypay_App.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<int, Employee> _employeeRepository;
        private readonly IRepository<int, DepartmentMaster> _departmentRepository;
        private readonly IRepository<int, RoleMaster> _roleRepository;
        private readonly IRepository<int, EmployeeStatusMaster> _statusRepository;
        private readonly IMapper _mapper;

        public EmployeeService(
            IRepository<int, Employee> employeeRepository,
            IRepository<int, DepartmentMaster> departmentRepository,
            IRepository<int, RoleMaster> roleRepository,
            IRepository<int, EmployeeStatusMaster> statusRepository,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _roleRepository = roleRepository;
            _statusRepository = statusRepository;
            _mapper = mapper;
        }

        public EmployeeAddResponseDTO AddEmployee(EmployeeAddRequestDTO dto)
        {
            #region 
            //You convert this DTO into your Employee entity, which matches your database structure.
            // AutoMapper does this automatically based on mappings you defined.
            #endregion
            var employee = _mapper.Map<Employee>(dto); //mapping dto to employee entity
            _employeeRepository.AddValue(employee);//Calls your repository layer, which in turn uses DbContext to save the employee to the database

            var response = _mapper.Map<EmployeeAddResponseDTO>(employee);
            PopulateNames(response, employee);
            return response;
        }

        public EmployeeAddResponseDTO UpdateEmployee(int id, EmployeeAddRequestDTO dto)
        {
            var existing = _employeeRepository.GetValueById(id);
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

            _employeeRepository.UpdateValue(existing.Id, existing);

            var response = _mapper.Map<EmployeeAddResponseDTO>(existing);
            PopulateNames(response, existing);
            return response;
        }

        public IEnumerable<EmployeeAddResponseDTO> GetAllEmployees()
        {
            var employees = _employeeRepository.GetAllValue();
            if (employees == null || !employees.Any())
                throw new NoItemFoundException();

            var response = employees.Select(emp =>
            {
                var dto = _mapper.Map<EmployeeAddResponseDTO>(emp);
                PopulateNames(dto, emp);
                return dto;
            }).ToList();

            return response;
        }

        public EmployeeAddResponseDTO GetEmployeeById(int id)
        {
            var employee = _employeeRepository.GetValueById(id);
            if (employee == null) throw new NoItemFoundException();

            var response = _mapper.Map<EmployeeAddResponseDTO>(employee);
            PopulateNames(response, employee);
            return response;
        }

        public EmployeeAddResponseDTO DeleteEmployee(int id)
        {
            var employee = _employeeRepository.GetValueById(id);
            if (employee == null) throw new NoItemFoundException();

            _employeeRepository.DeleteValue(id);

            var response = _mapper.Map<EmployeeAddResponseDTO>(employee);
            PopulateNames(response, employee);
            return response;
        }

        private void PopulateNames(EmployeeAddResponseDTO dto, Employee emp)
        {
            var dept = _departmentRepository.GetValueById(emp.DepartmentId);
            var role = _roleRepository.GetValueById(emp.RoleId);
            var status = _statusRepository.GetValueById(emp.StatusId);

            dto.DepartmentName = dept?.DepartmentName ?? "N/A";
            dto.RoleName = role?.RoleName ?? "N/A";
            dto.StatusName = status?.StatusName ?? "N/A";
            dto.ReportingManager = emp.ReportingManagerId;
        }
    }
}
