using AutoMapper;
using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;
using Easypay_App.Repositories;
using Easypay_App.Services;
using EasyPay_App.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Easypay_Test
{
    public class Tests
    {
        private IRepository<int, Employee> _employeeRepo;
        private IRepository<int, DepartmentMaster> _departmentRepo;
        private IRepository<int, RoleMaster> _roleRepo;
        private IRepository<int, EmployeeStatusMaster> _statusRepo;

        private Mock<IMapper> _mockMapper;
        private EmployeeService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PayrollContext>()
                .UseInMemoryDatabase("EmployeeServiceTestDB")
                .Options;

            var context = new PayrollContext(options);

            _employeeRepo = new EmployeeRepositoryDb(context);
            _departmentRepo = new DepartmentRepository(context);
            _roleRepo = new RoleRepository(context);
            _statusRepo = new EmployeeStatusRepository(context);

            // Seed Master Tables
            _departmentRepo.AddValue(new DepartmentMaster { Id = 1, DepartmentName = "HR" });
            _roleRepo.AddValue(new RoleMaster { Id = 1, RoleName = "Manager" });
            _statusRepo.AddValue(new EmployeeStatusMaster { Id = 1, StatusName = "Active" });

            _mockMapper = new Mock<IMapper>();

            _service = new EmployeeService(_employeeRepo, _departmentRepo, _roleRepo, _statusRepo, _mockMapper.Object);
        }

        [Test]
        public void AddEmployee()
        {
            // Arrange
            var dto = new EmployeeAddRequestDTO
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@test.com",
                PhoneNumber = "8888888888",
                DepartmentId = 1,
                RoleId = 1,
                StatusId = 1,
                Salary = 35000,
                ReportingManagerId = null
            };

            _mockMapper.Setup(m => m.Map<Employee>(dto)).Returns(new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DepartmentId = 1,
                RoleId = 1,
                StatusId = 1,
                Salary = dto.Salary
            });

            _mockMapper.Setup(m => m.Map<EmployeeAddResponseDTO>(It.IsAny<Employee>()))
                .Returns((Employee e) => new EmployeeAddResponseDTO
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber,
                    DepartmentName = "HR",      
                    RoleName = "Manager",       
                    StatusName = "Active"
                });

            // Act
            var result = _service.AddEmployee(dto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo("jane@test.com"));
        }

        [Test]
        public void GetEmployeeById()
        {

            // Add employee
            var employee = new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com",
                PhoneNumber = "9999999999",
                DepartmentId = 1,
                RoleId = 1,
                StatusId = 1,
                Salary = 30000
            };
            _employeeRepo.AddValue(employee);

            _mockMapper.Setup(m => m.Map<EmployeeAddResponseDTO>(employee)).Returns(new EmployeeAddResponseDTO
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber
            });

            // Act
            var result = _service.GetEmployeeById(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo("john@test.com"));
        }

        [Test]
        public void DeleteEmployee()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com",
                PhoneNumber = "9999999999",
                DepartmentId = 1,
                RoleId = 1,
                StatusId = 1
            };
            _employeeRepo.AddValue(employee);

            _mockMapper.Setup(m => m.Map<EmployeeAddResponseDTO>(employee)).Returns(new EmployeeAddResponseDTO
            {
                Id = employee.Id,
                Email = employee.Email
            });

            // Act
            var result = _service.DeleteEmployee(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public void DeleteEmployee_Exception()
        {
            // Add employee
            var employee = new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com",
                PhoneNumber = "9999999999",
                DepartmentId = 1,
                RoleId = 1,
                StatusId = 1
            };
            _employeeRepo.AddValue(employee);

            _mockMapper.Setup(m => m.Map<EmployeeAddResponseDTO>(employee)).Returns(new EmployeeAddResponseDTO
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber
            });

            // Act
            var result = _service.GetEmployeeById(1);
            Assert.Throws<NoItemFoundException>(() => _service.DeleteEmployee(999));
        }

        [Test]
        public void GetEmployeeById_Exception()
        {
            Assert.Throws<NoItemFoundException>(() => _service.GetEmployeeById(999));
        }
    }
}