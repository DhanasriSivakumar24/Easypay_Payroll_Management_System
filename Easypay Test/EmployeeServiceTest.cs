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
    public class EmployeeServiceTest
    {
        private IRepository<int, Employee> _employeeRepo;
        private IRepository<int, DepartmentMaster> _departmentRepo;
        private IRepository<int, RoleMaster> _roleRepo;
        private IRepository<int, EmployeeStatusMaster> _statusRepo;
        private IRepository<int, UserRoleMaster> _userRoleRepo;

        private Mock<IMapper> _mockMapper;
        private EmployeeService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PayrollContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Fresh DB for each test
                .Options;

            var context = new PayrollContext(options);

            _employeeRepo = new EmployeeRepository(context);
            _departmentRepo = new DepartmentRepository(context);
            _roleRepo = new RoleRepository(context);
            _statusRepo = new EmployeeStatusRepository(context);
            _userRoleRepo = new UserRoleRepository(context);

            _userRoleRepo.AddValue(new UserRoleMaster { Id = 1, UserRoleName = "Admin" });
            _userRoleRepo.AddValue(new UserRoleMaster { Id = 3, UserRoleName = "Employee" });

            _departmentRepo.AddValue(new DepartmentMaster { Id = 1, DepartmentName = "HR" });
            _roleRepo.AddValue(new RoleMaster { Id = 1, RoleName = "Manager" });
            _statusRepo.AddValue(new EmployeeStatusMaster { Id = 1, StatusName = "Active" });

            _mockMapper = new Mock<IMapper>();
            _service = new EmployeeService(_employeeRepo, _departmentRepo, _roleRepo, _statusRepo, _userRoleRepo, _mockMapper.Object);
        }

        #region AddEmployee
        [Test]
        public async Task AddEmployee()
        {
            var dto = new EmployeeAddRequestDTO
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@test.com",
                PhoneNumber = "8888888888",
                DepartmentId = 1,
                RoleId = 1,
                StatusId = 1,
                Salary = 35000
            };

            _mockMapper.Setup(m => m.Map<Employee>(dto)).Returns(new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DepartmentId = dto.DepartmentId,
                RoleId = dto.RoleId,
                StatusId = dto.StatusId,
                Salary = dto.Salary,
                UserRoleId = 3
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

            var result = await _service.AddEmployee(dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo("jane@test.com"));
        }
        #endregion

        #region GetEmployeeById
        [Test]
        public async Task GetEmployeeById()
        {
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
                UserRoleId = 1,
                Salary = 30000
            };

            await _employeeRepo.AddValue(employee);

            _mockMapper.Setup(m => m.Map<EmployeeAddResponseDTO>(employee))
                .Returns(new EmployeeAddResponseDTO
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber,
                    DepartmentName = "HR",
                    RoleName = "Manager",
                    StatusName = "Active",
                    UserRoleName = "Admin"
                });

            var result = await _service.GetEmployeeById(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo("john@test.com"));
        }
        #endregion

        #region GetEmployeeById_Exception
        [Test]
        public void GetEmployeeById_Exception()
        {
            Assert.ThrowsAsync<NoItemFoundException>(async () => await _service.GetEmployeeById(999));
        }

        [Test]
        public async Task DeleteEmployee()
        {
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
                UserRoleId = 3
            };

            await _employeeRepo.AddValue(employee);

            _mockMapper.Setup(m => m.Map<EmployeeAddResponseDTO>(employee))
                .Returns(new EmployeeAddResponseDTO
                {
                    Id = employee.Id,
                    Email = employee.Email,
                    UserRoleName = "Employee"
                });

            var result = await _service.DeleteEmployee(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
        }
        #endregion

        #region DeleteEmployee_Exception
        [Test]
        public async Task DeleteEmployee_Exception()
        {
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
                UserRoleId = 3
            };

            await _employeeRepo.AddValue(employee);

            Assert.ThrowsAsync<NoItemFoundException>(async () => await _service.DeleteEmployee(999));
        }
        #endregion

        #region UpdateEmployee_Exception
        [Test]
        public void UpdateEmployee_Exception()
        {
            var dto = new EmployeeAddRequestDTO
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@email.com",
                PhoneNumber = "0000000000",
                DepartmentId = 1,
                RoleId = 1,
                StatusId = 1,
                Salary = 40000
            };

            Assert.ThrowsAsync<NoItemFoundException>(() => _service.UpdateEmployee(999, dto));
        }
        #endregion

        # region UpdateEmployee
        [Test]
        public async Task UpdateEmployee()
        {
            // Arrange
            var existing = new Employee
            {
                Id = 1,
                FirstName = "Old",
                LastName = "Name",
                Email = "old@email.com",
                PhoneNumber = "1234567890",
                DepartmentId = 1,
                RoleId = 1,
                StatusId = 1,
                Salary = 25000,
                UserRoleId = 3
            };
            await _employeeRepo.AddValue(existing);

            var dto = new EmployeeAddRequestDTO
            {
                FirstName = "Updated",
                LastName = "User",
                Email = "updated@email.com",
                PhoneNumber = "9999999999",
                DepartmentId = 1,
                RoleId = 1,
                StatusId = 1,
                Salary = 40000
            };

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
            var result = await _service.UpdateEmployee(1, dto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.FirstName, Is.EqualTo("Updated"));
            Assert.That(result.Email, Is.EqualTo("updated@email.com"));
        }
        #endregion

        #region GetAllEmployees
        [Test]
        public async Task GetAllEmployees()
        {
            var emp1 = new Employee
            {
                Id = 1,
                FirstName = "Alice",
                LastName = "Brown",
                Email = "alice@email.com",
                PhoneNumber = "1111111111",
                DepartmentId = 1,
                RoleId = 1,
                StatusId = 1,
                UserRoleId = 3
            };
            var emp2 = new Employee
            {
                Id = 2,
                FirstName = "Bob",
                LastName = "Smith",
                Email = "bob@email.com",
                PhoneNumber = "2222222222",
                DepartmentId = 1,
                RoleId = 1,
                StatusId = 1,
                UserRoleId = 3
            };
            await _employeeRepo.AddValue(emp1);
            await _employeeRepo.AddValue(emp2);

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
                    StatusName = "Active",
                    UserRoleName = "Employee"
                });

            var result = await _service.GetAllEmployees();

            Assert.That(result.Count(), Is.EqualTo(2));
        }
        #endregion

        #region GetAllEmployees_Exception
        [Test]
        public void GetAllEmployees_Exception()
        {
            Assert.ThrowsAsync<NoItemFoundException>(async () => await _service.GetAllEmployees());
        }
        #endregion

        [TearDown]
        public void TearDown()
        {
            
        }
    }
}