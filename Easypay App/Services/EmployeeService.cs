using AutoMapper;
using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Interfaces;
using Easypay_App.Models;
using Easypay_App.Models.DTO;
using Microsoft.EntityFrameworkCore;

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
            var employee = _mapper.Map<Employee>(dto);
            _employeeRepository.AddValue(employee);

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

            _employeeRepository.UpdateValue(existing.Id,existing);

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


//#region
//using AutoMapper;
//using EasyPay_Payroll_Management_App.Context;
//using EasyPay_Payroll_Management_App.Exceptions;
//using EasyPay_Payroll_Management_App.Interfaces;
//using EasyPay_Payroll_Management_App.Models.DTO;
//using EasyPay_Payroll_Management_App.Models.Master;
//using EasyPay_Payroll_Management_App.Models.Transactional;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;

//namespace EasyPay_Payroll_Management_App.Services
//{
//    public class EmployeeService : IEmployeeService
//    {
//        private readonly IRepository<int, Employee> _employeeRepository;
//        private readonly IRepository<int, DepartmentMaster> _departmentRepository;
//        private readonly IRepository<int, RoleMaster> _roleRepository;
//        private readonly IRepository<int, EmployeeStatusMaster> _statusRepository;
//        private readonly PayrollManagementContext _context;
//        private readonly IMapper _mapper;

//        public EmployeeService(
//            IRepository<int, Employee> employeeRepository,
//            IRepository<int, DepartmentMaster> departmentRepository,
//            IRepository<int, RoleMaster> roleRepository,
//            IRepository<int, EmployeeStatusMaster> statusRepository,
//            PayrollManagementContext context,
//            IMapper mapper)
//        {
//            _employeeRepository = employeeRepository;
//            _departmentRepository = departmentRepository;
//            _roleRepository = roleRepository;
//            _statusRepository = statusRepository;
//            _context=context;
//            _mapper = mapper;
//        }

//        public EmployeeAddResponseDTO AddEmployee(AddEmployeeRequestDTO dto)
//        {
//            // 1. Map request DTO to Entity
//            var employee = _mapper.Map<Employee>(dto);

//            // 2. Save to database
//            _employeeRepository.AddValue(employee);

//            // 3. Reload saved employee with navigation properties using EF context
//            var savedEmployee = _context.Employee
//                .Include(e => e.Department)
//                .Include(e => e.Role)
//                .Include(e => e.Status)
//                .FirstOrDefault(e => e.Id == employee.Id);

//            // 4. Map to response DTO
//            var response = _mapper.Map<EmployeeAddResponseDTO>(savedEmployee);

//            // 5. Populate DepartmentName, RoleName, StatusName etc.
//            PopulateNames(response, savedEmployee);

//            return response;
//            //// 1. Map incoming DTO to entity
//            //var employee = _mapper.Map<Employee>(dto);

//            //// 2. Save to DB and get saved entity (with generated Id)
//            //var savedEmployee = _employeeRepository.AddValue(employee);

//            //// 3. Map saved entity to response DTO
//            //var response = _mapper.Map<EmployeeAddResponseDTO>(savedEmployee);

//            //// 4. Populate master names from related tables
//            //PopulateNames(response, savedEmployee);

//            //return response;
//        }

//        public EmployeeAddResponseDTO UpdateEmployee(int id, AddEmployeeRequestDTO dto)
//        {
//            var existing = _employeeRepository.GetValueById(id);
//            if (existing == null) throw new NoItemFoundException();

//            // Map updated values
//            existing.FirstName = dto.FirstName;
//            existing.LastName = dto.LastName;
//            existing.Email = dto.Email;
//            existing.PhoneNumber = dto.PhoneNumber;
//            existing.DepartmentId = dto.DepartmentId;
//            existing.RoleId = dto.RoleId;
//            existing.StatusId = dto.StatusId;
//            existing.ReportingManagerId = dto.ReportingManagerId;

//            _employeeRepository.UpdateValue(existing.Id, existing);

//            var response = _mapper.Map<EmployeeAddResponseDTO>(existing);
//            PopulateNames(response, existing);
//            return response;
//        }

//        public IEnumerable<EmployeeAddResponseDTO> GetAllEmployees()
//        {
//            var employees = _employeeRepository.GetAllValue();
//            if (employees == null || !employees.Any())
//                throw new NoItemFoundException();

//            var response = employees.Select(emp =>
//            {
//                var dto = _mapper.Map<EmployeeAddResponseDTO>(emp);
//                PopulateNames(dto, emp);
//                return dto;
//            }).ToList();

//            return response;
//        }

//        public EmployeeAddResponseDTO GetEmployeeById(int id)
//        {
//            var employee = _employeeRepository.GetValueById(id);
//            if (employee == null) throw new NoItemFoundException();

//            var response = _mapper.Map<EmployeeAddResponseDTO>(employee);
//            PopulateNames(response, employee);
//            return response;
//        }

//        public EmployeeAddResponseDTO DeleteEmployee(int id)
//        {
//            var employee = _employeeRepository.GetValueById(id);
//            if (employee == null) throw new NoItemFoundException();

//            _employeeRepository.DeleteValue(id);

//            var response = _mapper.Map<EmployeeAddResponseDTO>(employee);
//            PopulateNames(response, employee);
//            return response;
//        }

//        private void PopulateNames(EmployeeAddResponseDTO dto, Employee emp)
//        {
//            dto.DepartmentName = emp.Department?.DepartmentName ?? "N/A";
//            dto.RoleName = emp.Role?.RoleName ?? "N/A";
//            dto.StatusName = emp.Status?.StatusName ?? "N/A";
//            dto.ReportingManager = emp.ReportingManagerId;
//        }

//    }
//}
//#endregion