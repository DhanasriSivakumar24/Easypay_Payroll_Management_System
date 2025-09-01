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
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easypay_Test
{
    [TestFixture]
    public class EmployeeServiceTest
    {
        private IRepository<int, Employee> _employeeRepo;
        private IRepository<int, DepartmentMaster> _departmentRepo;
        private IRepository<int, RoleMaster> _roleRepo;
        private IRepository<int, EmployeeStatusMaster> _statusRepo;
        private IRepository<int, UserRoleMaster> _userRoleRepo;
        private Mock<IMapper> _mockMapper;
        private EmployeeService _service;
        private PayrollContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PayrollContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new PayrollContext(options);

            _employeeRepo = new EmployeeRepository(_context);
            _departmentRepo = new DepartmentRepository(_context);
            _roleRepo = new RoleRepository(_context);
            _statusRepo = new EmployeeStatusRepository(_context);
            _userRoleRepo = new UserRoleRepository(_context);

            _mockMapper = new Mock<IMapper>();
            _service = new EmployeeService(_employeeRepo, _departmentRepo, _roleRepo, _statusRepo, _userRoleRepo, _mockMapper.Object);

            var departments = new List<DepartmentMaster>
            {
                new DepartmentMaster { Id = 1, DepartmentName = "HR" },
                new DepartmentMaster { Id = 2, DepartmentName = "IT" }
            };
            var roles = new List<RoleMaster>
            {
                new RoleMaster { Id = 1, RoleName = "Manager" },
                new RoleMaster { Id = 2, RoleName = "Developer" }
            };
            var statuses = new List<EmployeeStatusMaster>
            {
                new EmployeeStatusMaster { Id = 1, StatusName = "Active" },
                new EmployeeStatusMaster { Id = 2, StatusName = "Inactive" }
            };
            var userRoles = new List<UserRoleMaster>
            {
                new UserRoleMaster { Id = 1, UserRoleName = "Admin" },
                new UserRoleMaster { Id = 2, UserRoleName = "Employee" }
            };

            _context.DepartmentMasters.AddRange(departments);
            _context.RoleMasters.AddRange(roles);
            _context.EmployeeStatusMasters.AddRange(statuses);
            _context.UserRoleMasters.AddRange(userRoles);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private void SetupMapperForEmployee(Employee employee)
        {
            _mockMapper.Setup(m => m.Map<EmployeeAddResponseDTO>(It.Is<Employee>(e => e.Id == employee.Id)))
                .Returns(new EmployeeAddResponseDTO
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber,
                    DepartmentName = employee.DepartmentId == 1 ? "HR" : "IT",
                    RoleName = employee.RoleId == 1 ? "Manager" : "Developer",
                    StatusName = employee.StatusId == 1 ? "Active" : "Inactive",
                    UserRoleName = employee.UserRoleId == 1 ? "Admin" : "Employee",
                    Salary = employee.Salary,
                    ReportingManager = employee.ReportingManagerId,
                    ReportingManagerName = employee.ReportingManagerId.HasValue ? "Manager Name" : "N/A",
                    DateOfBirth = employee.DateOfBirth,
                    JoinDate = employee.JoinDate,
                    Address = employee.Address ?? string.Empty,
                    PanNumber = employee.PanNumber ?? string.Empty,
                    Gender = employee.Gender ?? string.Empty
                });
        }

        #region AddEmployee
        [Test]
        public async Task AddEmployee_ShouldReturnResponse_WhenValid()
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
                Salary = 35000,
                ReportingManagerId = null,
                UserRoleId = 2,
                DateOfBirth = new DateTime(1990, 1, 1),
                JoinDate = new DateTime(2020, 1, 1),
                Address = "123 Main St",
                PanNumber = "ABCDE1234F",
                Gender = "F"
            };

            var employee = new Employee
            {
                Id = 1,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DepartmentId = dto.DepartmentId,
                RoleId = dto.RoleId,
                StatusId = dto.StatusId,
                Salary = dto.Salary,
                ReportingManagerId = dto.ReportingManagerId,
                UserRoleId = dto.UserRoleId,
                DateOfBirth = dto.DateOfBirth,
                JoinDate = dto.JoinDate,
                Address = dto.Address,
                PanNumber = dto.PanNumber,
                Gender = dto.Gender
            };

            _mockMapper.Setup(m => m.Map<Employee>(dto)).Returns(employee);
            SetupMapperForEmployee(employee);

            var result = await _service.AddEmployee(dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo("jane@test.com"));
            Assert.That(result.FirstName, Is.EqualTo("Jane"));
            Assert.That(result.LastName, Is.EqualTo("Smith"));
            Assert.That(result.DepartmentName, Is.EqualTo("HR"));
            Assert.That(result.RoleName, Is.EqualTo("Manager"));
            Assert.That(result.StatusName, Is.EqualTo("Active"));
            Assert.That(result.UserRoleName, Is.EqualTo("Employee"));
            Assert.That(result.Salary, Is.EqualTo(35000));
            Assert.That(result.DateOfBirth, Is.EqualTo(new DateTime(1990, 1, 1)));
            Assert.That(result.JoinDate, Is.EqualTo(new DateTime(2020, 1, 1)));
            Assert.That(result.Address, Is.EqualTo("123 Main St"));
            Assert.That(result.PanNumber, Is.EqualTo("ABCDE1234F"));
            Assert.That(result.Gender, Is.EqualTo("F"));
        }
        #endregion

        #region UpdateEmployee
        [Test]
        public async Task UpdateEmployee_ShouldReturnResponse_WhenValid()
        {
            var employee = new Employee
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
                UserRoleId = 2,
                DateOfBirth = new DateTime(1980, 1, 1),
                JoinDate = new DateTime(2020, 1, 1),
                Address = "Old Address",
                PanNumber = "ABCDE1234F",
                Gender = "M"
            };
            await _employeeRepo.AddValue(employee);

            var dto = new EmployeeUpdateRequestDTO
            {
                FirstName = "Updated",
                LastName = "User",
                Email = "updated@email.com",
                PhoneNumber = "9999999999",
                DepartmentId = 2,
                RoleId = 2,
                StatusId = 1,
                Salary = 40000,
                ReportingManagerId = 1,
                UserRoleId = 1,
                DateOfBirth = new DateTime(1985, 5, 5),
                JoinDate = new DateTime(2021, 2, 2),
                Address = "New Address",
                PanNumber = "XYZAB5678G",
                Gender = "F"
            };

            var updatedEmployee = new Employee
            {
                Id = 1,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DepartmentId = dto.DepartmentId!.Value,
                RoleId = dto.RoleId!.Value,
                StatusId = dto.StatusId!.Value,
                Salary = dto.Salary!.Value,
                ReportingManagerId = dto.ReportingManagerId,
                UserRoleId = dto.UserRoleId!.Value,
                DateOfBirth = dto.DateOfBirth!.Value,
                JoinDate = dto.JoinDate!.Value,
                Address = dto.Address,
                PanNumber = dto.PanNumber,
                Gender = dto.Gender
            };

            _mockMapper.Setup(m => m.Map<EmployeeAddResponseDTO>(It.Is<Employee>(e => e.Id == updatedEmployee.Id)))
                .Returns(new EmployeeAddResponseDTO
                {
                    Id = updatedEmployee.Id,
                    FirstName = updatedEmployee.FirstName,
                    LastName = updatedEmployee.LastName,
                    Email = updatedEmployee.Email,
                    PhoneNumber = updatedEmployee.PhoneNumber,
                    DepartmentName = updatedEmployee.DepartmentId == 1 ? "HR" : "IT",
                    RoleName = updatedEmployee.RoleId == 1 ? "Manager" : "Developer",
                    StatusName = updatedEmployee.StatusId == 1 ? "Active" : "Inactive",
                    UserRoleName = updatedEmployee.UserRoleId == 1 ? "Admin" : "Employee",
                    Salary = updatedEmployee.Salary,
                    ReportingManager = updatedEmployee.ReportingManagerId,
                    ReportingManagerName = updatedEmployee.ReportingManagerId.HasValue ? "Manager Name" : "N/A",
                    DateOfBirth = updatedEmployee.DateOfBirth,
                    JoinDate = updatedEmployee.JoinDate,
                    Address = updatedEmployee.Address ?? string.Empty,
                    PanNumber = updatedEmployee.PanNumber ?? string.Empty,
                    Gender = updatedEmployee.Gender ?? string.Empty
                });

            var result = await _service.UpdateEmployee(1, dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.FirstName, Is.EqualTo("Updated"));
            Assert.That(result.LastName, Is.EqualTo("User"));
            Assert.That(result.Email, Is.EqualTo("updated@email.com"));
            Assert.That(result.DepartmentName, Is.EqualTo("IT"));
            Assert.That(result.RoleName, Is.EqualTo("Developer"));
            Assert.That(result.UserRoleName, Is.EqualTo("Admin"));
            Assert.That(result.ReportingManager, Is.EqualTo(1));
            Assert.That(result.Salary, Is.EqualTo(40000));
            Assert.That(result.DateOfBirth, Is.EqualTo(new DateTime(1985, 5, 5)));
            Assert.That(result.JoinDate, Is.EqualTo(new DateTime(2021, 2, 2)));
            Assert.That(result.Address, Is.EqualTo("New Address"));
            Assert.That(result.PanNumber, Is.EqualTo("XYZAB5678G"));
            Assert.That(result.Gender, Is.EqualTo("F"));
        }

        [Test]
        public async Task UpdateEmployee_ShouldUpdateOnlyProvidedFields_WhenPartialUpdate()
        {
            var employee = new Employee
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
                UserRoleId = 2,
                DateOfBirth = new DateTime(1980, 1, 1),
                JoinDate = new DateTime(2020, 1, 1),
                Address = "Old Address",
                PanNumber = "ABCDE1234F",
                Gender = "M"
            };
            await _employeeRepo.AddValue(employee);

            var dto = new EmployeeUpdateRequestDTO
            {
                FirstName = "Updated",
                Salary = 35000
            };

            var updatedEmployee = new Employee
            {
                Id = 1,
                FirstName = "Updated",
                LastName = employee.LastName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                DepartmentId = employee.DepartmentId,
                RoleId = employee.RoleId,
                StatusId = employee.StatusId,
                Salary = 35000,
                UserRoleId = employee.UserRoleId,
                DateOfBirth = employee.DateOfBirth,
                JoinDate = employee.JoinDate,
                Address = employee.Address,
                PanNumber = employee.PanNumber,
                Gender = employee.Gender
            };

            _mockMapper.Setup(m => m.Map<EmployeeAddResponseDTO>(It.Is<Employee>(e => e.Id == updatedEmployee.Id)))
                .Returns(new EmployeeAddResponseDTO
                {
                    Id = updatedEmployee.Id,
                    FirstName = updatedEmployee.FirstName,
                    LastName = updatedEmployee.LastName,
                    Email = updatedEmployee.Email,
                    PhoneNumber = updatedEmployee.PhoneNumber,
                    DepartmentName = updatedEmployee.DepartmentId == 1 ? "HR" : "IT",
                    RoleName = updatedEmployee.RoleId == 1 ? "Manager" : "Developer",
                    StatusName = updatedEmployee.StatusId == 1 ? "Active" : "Inactive",
                    UserRoleName = updatedEmployee.UserRoleId == 1 ? "Admin" : "Employee",
                    Salary = updatedEmployee.Salary,
                    ReportingManager = updatedEmployee.ReportingManagerId,
                    ReportingManagerName = updatedEmployee.ReportingManagerId.HasValue ? "Manager Name" : "N/A",
                    DateOfBirth = updatedEmployee.DateOfBirth,
                    JoinDate = updatedEmployee.JoinDate,
                    Address = updatedEmployee.Address ?? string.Empty,
                    PanNumber = updatedEmployee.PanNumber ?? string.Empty,
                    Gender = updatedEmployee.Gender ?? string.Empty
                });

            var result = await _service.UpdateEmployee(1, dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.FirstName, Is.EqualTo("Updated"));
            Assert.That(result.LastName, Is.EqualTo("Name"));
            Assert.That(result.Email, Is.EqualTo("old@email.com"));
            Assert.That(result.Salary, Is.EqualTo(35000));
            Assert.That(result.DepartmentName, Is.EqualTo("HR"));
            Assert.That(result.RoleName, Is.EqualTo("Manager"));
            Assert.That(result.UserRoleName, Is.EqualTo("Employee"));
            Assert.That(result.DateOfBirth, Is.EqualTo(new DateTime(1980, 1, 1)));
            Assert.That(result.JoinDate, Is.EqualTo(new DateTime(2020, 1, 1)));
            Assert.That(result.Address, Is.EqualTo("Old Address"));
            Assert.That(result.PanNumber, Is.EqualTo("ABCDE1234F"));
            Assert.That(result.Gender, Is.EqualTo("M"));
        }

        [Test]
        public void UpdateEmployee_ShouldThrowException_WhenEmployeeNotFound()
        {
            var dto = new EmployeeUpdateRequestDTO
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

        #region GetAllEmployees
        [Test]
        public async Task GetAllEmployees_ShouldReturnList_WhenFound()
        {
            var employees = new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    FirstName = "Alice",
                    LastName = "Brown",
                    Email = "alice@email.com",
                    PhoneNumber = "1111111111",
                    DepartmentId = 1,
                    RoleId = 1,
                    StatusId = 1,
                    UserRoleId = 2,
                    Salary = 30000,
                    DateOfBirth = new DateTime(1990, 1, 1),
                    JoinDate = new DateTime(2020, 1, 1),
                    Address = "456 Oak St",
                    PanNumber = "FGHIJ6789K",
                    Gender = "F"
                },
                new Employee
                {
                    Id = 2,
                    FirstName = "Bob",
                    LastName = "Smith",
                    Email = "bob@email.com",
                    PhoneNumber = "2222222222",
                    DepartmentId = 2,
                    RoleId = 2,
                    StatusId = 1,
                    UserRoleId = 1,
                    Salary = 40000,
                    DateOfBirth = new DateTime(1985, 2, 2),
                    JoinDate = new DateTime(2019, 2, 2),
                    Address = "789 Pine St",
                    PanNumber = "KLMNO1234P",
                    Gender = "M"
                }
            };
            foreach (var emp in employees)
            {
                await _employeeRepo.AddValue(emp);
                SetupMapperForEmployee(emp);
            }

            var result = await _service.GetAllEmployees();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.Any(r => r.FirstName == "Alice" && r.DepartmentName == "HR"), Is.True);
            Assert.That(result.Any(r => r.FirstName == "Bob" && r.DepartmentName == "IT"), Is.True);
        }

        [Test]
        public void GetAllEmployees_ShouldThrowException_WhenNoneFound()
        {
            Assert.ThrowsAsync<NoItemFoundException>(() => _service.GetAllEmployees());
        }
        #endregion

        #region GetEmployeeById
        [Test]
        public async Task GetEmployeeById_ShouldReturnResponse_WhenValid()
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
                Salary = 30000,
                DateOfBirth = new DateTime(1990, 1, 1),
                JoinDate = new DateTime(2020, 1, 1),
                Address = "123 Main St",
                PanNumber = "ABCDE1234F",
                Gender = "M"
            };
            await _employeeRepo.AddValue(employee);
            SetupMapperForEmployee(employee);

            var result = await _service.GetEmployeeById(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Email, Is.EqualTo("john@test.com"));
            Assert.That(result.DepartmentName, Is.EqualTo("HR"));
            Assert.That(result.RoleName, Is.EqualTo("Manager"));
            Assert.That(result.StatusName, Is.EqualTo("Active"));
            Assert.That(result.UserRoleName, Is.EqualTo("Admin"));
            Assert.That(result.Salary, Is.EqualTo(30000));
            Assert.That(result.DateOfBirth, Is.EqualTo(new DateTime(1990, 1, 1)));
            Assert.That(result.JoinDate, Is.EqualTo(new DateTime(2020, 1, 1)));
            Assert.That(result.Address, Is.EqualTo("123 Main St"));
            Assert.That(result.PanNumber, Is.EqualTo("ABCDE1234F"));
            Assert.That(result.Gender, Is.EqualTo("M"));
        }

        [Test]
        public void GetEmployeeById_ShouldThrowException_WhenEmployeeNotFound()
        {
            Assert.ThrowsAsync<NoItemFoundException>(() => _service.GetEmployeeById(999));
        }
        #endregion

        #region DeleteEmployee
        [Test]
        public async Task DeleteEmployee_ShouldReturnResponse_WhenValid()
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
                UserRoleId = 2,
                Salary = 30000,
                DateOfBirth = new DateTime(1990, 1, 1),
                JoinDate = new DateTime(2020, 1, 1),
                Address = "123 Main St",
                PanNumber = "ABCDE1234F",
                Gender = "M"
            };
            await _employeeRepo.AddValue(employee);
            SetupMapperForEmployee(employee);

            var result = await _service.DeleteEmployee(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Email, Is.EqualTo("john@test.com"));
            Assert.That(result.UserRoleName, Is.EqualTo("Employee"));
        }

        [Test]
        public void DeleteEmployee_ShouldThrowException_WhenEmployeeNotFound()
        {
            Assert.ThrowsAsync<NoItemFoundException>(() => _service.DeleteEmployee(999));
        }
        #endregion

        #region ChangeEmployeeUserRole
        [Test]
        public async Task ChangeEmployeeUserRole_ShouldReturnResponse_WhenValid()
        {
            var employee = new Employee
            {
                Id = 1,
                FirstName = "Ravi",
                LastName = "Kumar",
                Email = "ravi@test.com",
                PhoneNumber = "9876543210",
                DepartmentId = 1,
                RoleId = 1,
                StatusId = 1,
                Salary = 42000,
                UserRoleId = 2,
                DateOfBirth = new DateTime(1990, 1, 1),
                JoinDate = new DateTime(2020, 1, 1),
                Address = "123 Main St",
                PanNumber = "ABCDE1234F",
                Gender = "M"
            };
            await _employeeRepo.AddValue(employee);

            var dto = new ChangeUserRoleDTO
            {
                EmployeeId = 1,
                NewUserRoleId = 1
            };

            SetupMapperForEmployee(new Employee
            {
                Id = 1,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                DepartmentId = employee.DepartmentId,
                RoleId = employee.RoleId,
                StatusId = employee.StatusId,
                Salary = employee.Salary,
                UserRoleId = 1,
                DateOfBirth = employee.DateOfBirth,
                JoinDate = employee.JoinDate,
                Address = employee.Address,
                PanNumber = employee.PanNumber,
                Gender = employee.Gender
            });

            var result = await _service.ChangeEmployeeUserRole(dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserRoleName, Is.EqualTo("Admin"));
            var updatedEmployee = await _employeeRepo.GetValueById(1);
            Assert.That(updatedEmployee.UserRoleId, Is.EqualTo(1));
        }

        [Test]
        public void ChangeEmployeeUserRole_ShouldThrowException_WhenEmployeeNotFound()
        {
            var dto = new ChangeUserRoleDTO
            {
                EmployeeId = 999,
                NewUserRoleId = 1
            };

            Assert.ThrowsAsync<NoItemFoundException>(() => _service.ChangeEmployeeUserRole(dto));
        }
        #endregion

        #region SearchEmployees
        [Test]
        public async Task SearchEmployees_ShouldReturnPaginatedResponse_WhenFoundByFirstName()
        {
            var employees = new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    FirstName = "Alice",
                    LastName = "Brown",
                    Email = "alice@email.com",
                    PhoneNumber = "1111111111",
                    DepartmentId = 1,
                    RoleId = 1,
                    StatusId = 1,
                    UserRoleId = 2,
                    Salary = 30000,
                    DateOfBirth = new DateTime(1990, 1, 1),
                    JoinDate = new DateTime(2020, 1, 1),
                    Address = "456 Oak St",
                    PanNumber = "FGHIJ6789K",
                    Gender = "F"
                },
                new Employee
                {
                    Id = 2,
                    FirstName = "Bob",
                    LastName = "Smith",
                    Email = "bob@email.com",
                    PhoneNumber = "2222222222",
                    DepartmentId = 2,
                    RoleId = 2,
                    StatusId = 1,
                    UserRoleId = 1,
                    Salary = 40000,
                    DateOfBirth = new DateTime(1985, 2, 2),
                    JoinDate = new DateTime(2019, 2, 2),
                    Address = "789 Pine St",
                    PanNumber = "KLMNO1234P",
                    Gender = "M"
                }
            };
            foreach (var emp in employees)
            {
                await _employeeRepo.AddValue(emp);
                SetupMapperForEmployee(emp);
            }

            var criteria = new EmployeeSearchRequestDTO
            {
                FirstName = "Alice",
                PageNumber = 1,
                PageSize = 10
            };

            var result = await _service.SearchEmployees(criteria);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Employees.Count, Is.EqualTo(1));
            Assert.That(result.TotalNumberOfRecords, Is.EqualTo(1));
            Assert.That(result.Employees.First().FirstName, Is.EqualTo("Alice"));
            Assert.That(result.Employees.First().DepartmentName, Is.EqualTo("HR"));
        }

        [Test]
        public async Task SearchEmployees_ShouldReturnPaginatedResponse_WhenFoundByDepartment()
        {
            var employees = new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    FirstName = "Alice",
                    LastName = "Brown",
                    Email = "alice@email.com",
                    PhoneNumber = "1111111111",
                    DepartmentId = 1,
                    RoleId = 1,
                    StatusId = 1,
                    UserRoleId = 2,
                    Salary = 30000,
                    DateOfBirth = new DateTime(1990, 1, 1),
                    JoinDate = new DateTime(2020, 1, 1),
                    Address = "456 Oak St",
                    PanNumber = "FGHIJ6789K",
                    Gender = "F"
                },
                new Employee
                {
                    Id = 2,
                    FirstName = "Bob",
                    LastName = "Smith",
                    Email = "bob@email.com",
                    PhoneNumber = "2222222222",
                    DepartmentId = 2,
                    RoleId = 2,
                    StatusId = 1,
                    UserRoleId = 1,
                    Salary = 40000,
                    DateOfBirth = new DateTime(1985, 2, 2),
                    JoinDate = new DateTime(2019, 2, 2),
                    Address = "789 Pine St",
                    PanNumber = "KLMNO1234P",
                    Gender = "M"
                }
            };
            foreach (var emp in employees)
            {
                await _employeeRepo.AddValue(emp);
                SetupMapperForEmployee(emp);
            }

            var criteria = new EmployeeSearchRequestDTO
            {
                Departments = new List<int> { 1 },
                PageNumber = 1,
                PageSize = 10
            };

            var result = await _service.SearchEmployees(criteria);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Employees.Count, Is.EqualTo(1));
            Assert.That(result.TotalNumberOfRecords, Is.EqualTo(1));
            Assert.That(result.Employees.First().FirstName, Is.EqualTo("Alice"));
            Assert.That(result.Employees.First().DepartmentName, Is.EqualTo("HR"));
        }

        [Test]
        public async Task SearchEmployees_ShouldReturnPaginatedResponse_WhenSorted()
        {
            var employees = new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    FirstName = "Bob",
                    LastName = "Smith",
                    Email = "bob@email.com",
                    PhoneNumber = "2222222222",
                    DepartmentId = 2,
                    RoleId = 2,
                    StatusId = 1,
                    UserRoleId = 1,
                    Salary = 40000,
                    DateOfBirth = new DateTime(1985, 2, 2),
                    JoinDate = new DateTime(2019, 2, 2),
                    Address = "789 Pine St",
                    PanNumber = "KLMNO1234P",
                    Gender = "M"
                },
                new Employee
                {
                    Id = 2,
                    FirstName = "Alice",
                    LastName = "Brown",
                    Email = "alice@email.com",
                    PhoneNumber = "1111111111",
                    DepartmentId = 1,
                    RoleId = 1,
                    StatusId = 1,
                    UserRoleId = 2,
                    Salary = 30000,
                    DateOfBirth = new DateTime(1990, 1, 1),
                    JoinDate = new DateTime(2020, 1, 1),
                    Address = "456 Oak St",
                    PanNumber = "FGHIJ6789K",
                    Gender = "F"
                }
            };
            foreach (var emp in employees)
            {
                await _employeeRepo.AddValue(emp);
                SetupMapperForEmployee(emp);
            }

            var criteria = new EmployeeSearchRequestDTO
            {
                Sort = 2,
                PageNumber = 1,
                PageSize = 10
            };

            var result = await _service.SearchEmployees(criteria);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Employees.Count, Is.EqualTo(2));
            Assert.That(result.TotalNumberOfRecords, Is.EqualTo(2));
            Assert.That(result.Employees.First().FirstName, Is.EqualTo("Alice"));
            Assert.That(result.Employees.Last().FirstName, Is.EqualTo("Bob"));
        }

        [Test]
        public async Task SearchEmployees_ShouldReturnPaginatedResponse()
        {
            var employees = new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    FirstName = "Alice",
                    LastName = "Brown",
                    Email = "alice@email.com",
                    PhoneNumber = "1111111111",
                    DepartmentId = 1,
                    RoleId = 1,
                    StatusId = 1,
                    UserRoleId = 2,
                    Salary = 30000,
                    DateOfBirth = new DateTime(1990, 1, 1),
                    JoinDate = new DateTime(2020, 1, 1),
                    Address = "456 Oak St",
                    PanNumber = "FGHIJ6789K",
                    Gender = "F"
                },
                new Employee
                {
                    Id = 2,
                    FirstName = "Bob",
                    LastName = "Smith",
                    Email = "bob@email.com",
                    PhoneNumber = "2222222222",
                    DepartmentId = 2,
                    RoleId = 2,
                    StatusId = 1,
                    UserRoleId = 1,
                    Salary = 50000,
                    DateOfBirth = new DateTime(1985, 2, 2),
                    JoinDate = new DateTime(2019, 2, 2),
                    Address = "789 Pine St",
                    PanNumber = "KLMNO1234P",
                    Gender = "M"
                }
            };
            foreach (var emp in employees)
            {
                await _employeeRepo.AddValue(emp);
                SetupMapperForEmployee(emp);
            }

            var criteria = new EmployeeSearchRequestDTO
            {
                SalaryRange = new SearchRange<decimal> { MinValue = 20000, MaxValue = 35000 },
                PageNumber = 1,
                PageSize = 10
            };

            var result = await _service.SearchEmployees(criteria);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Employees.Count, Is.EqualTo(1));
            Assert.That(result.TotalNumberOfRecords, Is.EqualTo(1));
            Assert.That(result.Employees.First().FirstName, Is.EqualTo("Alice"));
        }

        [Test]
        public void SearchEmployees_ShouldThrowException()
        {
            var criteria = new EmployeeSearchRequestDTO
            {
                FirstName = "NonExistent",
                PageNumber = 1,
                PageSize = 10
            };

            var exception = Assert.ThrowsAsync<Exception>(() => _service.SearchEmployees(criteria));
            Assert.That(exception.Message, Is.EqualTo("No search result Found"));
        }
        #endregion
    }
}