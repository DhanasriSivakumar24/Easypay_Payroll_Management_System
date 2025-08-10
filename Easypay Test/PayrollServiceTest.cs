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
    public class PayrollServiceTest
    {
        private IRepository<int, Employee> _employeeRepo;
        private IRepository<int, PayrollPolicyMaster> _policyRepo;
        private IRepository<int, PayrollStatusMaster> _statusRepo;
        private IRepository<int, Payroll> _payrollRepo;
        private IRepository<int, Timesheet> _timesheetRepo;
        private Mock<IMapper> _mockMapper;
        private PayrollService _service;
        private PayrollContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PayrollContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new PayrollContext(options);

            _employeeRepo = new EmployeeRepository(_context);
            _policyRepo = new PayrollPolicyRepository(_context);
            _statusRepo = new PayrollStatusRepository(_context);
            _payrollRepo = new PayrollRepository(_context);
            _timesheetRepo = new TimesheetRepository(_context);

            _mockMapper = new Mock<IMapper>();
            _service = new PayrollService(_employeeRepo, _policyRepo, _statusRepo, _payrollRepo, _timesheetRepo, _mockMapper.Object);

            // Seed data
            var employees = new List<Employee>
            {
                new Employee { Id = 1, FirstName = "John", LastName = "Doe", Salary = 50000 },
                new Employee { Id = 2, FirstName = "Jane", LastName = "Smith", Salary = 60000 }
            };
            _context.Employees.AddRange(employees);

            var policies = new List<PayrollPolicyMaster>
            {
                new PayrollPolicyMaster
                {
                    Id = 1,
                    PolicyName = "Standard",
                    BasicPercent = 50,
                    HRAPercent = 20,
                    SpecialPercent = 10,
                    TravelPercent = 10,
                    MedicalPercent = 5,
                    EmployeePercent = 5,
                    EmployerPercent = 5,
                    GratuityPercent = 2
                }
            };
            _context.PayrollPolicyMasters.AddRange(policies);

            var statuses = new List<PayrollStatusMaster>
            {
                new PayrollStatusMaster { Id = 1, StatusName = "Generated" },
                new PayrollStatusMaster { Id = 2, StatusName = "Processed" },
                new PayrollStatusMaster { Id = 3, StatusName = "Approved" }
            };
            _context.PayrollStatusMasters.AddRange(statuses);

            var timesheets = new List<Timesheet>
            {
                new Timesheet { Id = 1, EmployeeId = 1, WorkDate = DateTime.Now.AddDays(-1), HoursWorked = 8, StatusId = 1 }
            };
            _context.Timesheets.AddRange(timesheets);

            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        #region ApprovePayroll
        [Test]
        public async Task ApprovePayroll_ShouldReturnResponse_WhenValid()
        {
            var payroll = new Payroll
            {
                Id = 1,
                EmployeeId = 1,
                PolicyId = 1,
                PeriodStart = DateTime.Today.AddDays(-7),
                PeriodEnd = DateTime.Today,
                StatusId = 1,
                NetPay = 45000
            };
            await _payrollRepo.AddValue(payroll);

            _mockMapper.Setup(m => m.Map<PayrollResponseDTO>(It.IsAny<Payroll>()))
                .Returns((Payroll p) => new PayrollResponseDTO
                {
                    Id = p.Id,
                    EmployeeName = "John Doe",
                    PolicyName = "Standard",
                    StatusName = "Approved",
                    NetPay = p.NetPay,
                    PeriodStart = p.PeriodStart,
                    PeriodEnd = p.PeriodEnd
                });

            var result = await _service.ApprovePayroll(payroll.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Approved", result.StatusName);
            var updatedPayroll = await _payrollRepo.GetValueById(payroll.Id);
            Assert.AreEqual(3, updatedPayroll.StatusId);
        }

        [Test]
        public void ApprovePayroll_ShouldThrowException_WhenPayrollNotFound()
        {
            Assert.ThrowsAsync<NoItemFoundException>(() => _service.ApprovePayroll(999));
        }
        #endregion

        #region GetApprovedPayrolls
        [Test]
        public async Task GetApprovedPayrolls_ShouldReturnList_WhenFound()
        {
            var payrolls = new List<Payroll>
            {
                new Payroll { Id = 1, EmployeeId = 1, PolicyId = 1, PeriodStart = DateTime.Today.AddDays(-7), PeriodEnd = DateTime.Today, StatusId = 3, NetPay = 45000 },
                new Payroll { Id = 2, EmployeeId = 2, PolicyId = 1, PeriodStart = DateTime.Today.AddDays(-7), PeriodEnd = DateTime.Today, StatusId = 3, NetPay = 55000 }
            };
            foreach (var p in payrolls)
            {
                await _payrollRepo.AddValue(p);
            }

            _mockMapper.Setup(m => m.Map<PayrollResponseDTO>(It.IsAny<Payroll>()))
                .Returns((Payroll p) => new PayrollResponseDTO
                {
                    Id = p.Id,
                    EmployeeName = p.EmployeeId == 1 ? "John Doe" : "Jane Smith",
                    PolicyName = "Standard",
                    StatusName = "Approved",
                    NetPay = p.NetPay,
                    PeriodStart = p.PeriodStart,
                    PeriodEnd = p.PeriodEnd
                });

            var result = await _service.GetApprovedPayrolls(DateTime.Today.AddDays(-7), DateTime.Today);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(r => r.EmployeeName == "John Doe" && r.NetPay == 45000));
            Assert.IsTrue(result.Any(r => r.EmployeeName == "Jane Smith" && r.NetPay == 55000));
        }

        [Test]
        public async Task GetApprovedPayrolls_ShouldReturnEmptyList_WhenNoneFound()
        {
            var result = await _service.GetApprovedPayrolls(DateTime.Today.AddDays(-7), DateTime.Today);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
        #endregion

        #region GeneratePayroll
        [Test]
        public async Task GeneratePayroll_ShouldReturnResponse_WhenValid()
        {
            var requestDto = new PayrollRequestDTO
            {
                EmployeeId = 1,
                PolicyId = 1,
                PeriodStart = DateTime.Today.AddDays(-7),
                PeriodEnd = DateTime.Today
            };

            _mockMapper.Setup(m => m.Map<PayrollResponseDTO>(It.IsAny<Payroll>()))
                .Returns((Payroll p) => new PayrollResponseDTO
                {
                    Id = p.Id,
                    EmployeeName = "John Doe",
                    PolicyName = "Standard",
                    StatusName = "Generated",
                    NetPay = p.NetPay,
                    PeriodStart = p.PeriodStart,
                    PeriodEnd = p.PeriodEnd
                });

            var result = await _service.GeneratePayroll(requestDto);

            Assert.IsNotNull(result);
            Assert.AreEqual("John Doe", result.EmployeeName);
            Assert.AreEqual("Generated", result.StatusName);
            Assert.IsTrue(result.NetPay > 0); // NetPay depends on timesheet calculations, so we check it's positive
        }

        [Test]
        public void GeneratePayroll_ShouldThrowException_WhenEmployeeOrPolicyNotFound()
        {
            var requestDto = new PayrollRequestDTO
            {
                EmployeeId = 999,
                PolicyId = 999,
                PeriodStart = DateTime.Today.AddDays(-7),
                PeriodEnd = DateTime.Today
            };

            Assert.ThrowsAsync<NoItemFoundException>(() => _service.GeneratePayroll(requestDto));
        }
        #endregion

        #region GetAllPayrolls
        [Test]
        public async Task GetAllPayrolls_ShouldReturnList_WhenFound()
        {
            var payrolls = new List<Payroll>
            {
                new Payroll { Id = 1, EmployeeId = 1, PolicyId = 1, PeriodStart = DateTime.Today.AddDays(-7), PeriodEnd = DateTime.Today, StatusId = 1, NetPay = 45000 },
                new Payroll { Id = 2, EmployeeId = 2, PolicyId = 1, PeriodStart = DateTime.Today.AddDays(-7), PeriodEnd = DateTime.Today, StatusId = 1, NetPay = 55000 }
            };
            foreach (var p in payrolls)
            {
                await _payrollRepo.AddValue(p);
            }

            _mockMapper.Setup(m => m.Map<PayrollResponseDTO>(It.IsAny<Payroll>()))
                .Returns((Payroll p) => new PayrollResponseDTO
                {
                    Id = p.Id,
                    EmployeeName = p.EmployeeId == 1 ? "John Doe" : "Jane Smith",
                    PolicyName = "Standard",
                    StatusName = "Generated",
                    NetPay = p.NetPay,
                    PeriodStart = p.PeriodStart,
                    PeriodEnd = p.PeriodEnd
                });

            var result = await _service.GetAllPayrolls();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(r => r.EmployeeName == "John Doe"));
            Assert.IsTrue(result.Any(r => r.EmployeeName == "Jane Smith"));
        }

        [Test]
        public async Task GetAllPayrolls_ShouldReturnEmptyList_WhenNoneFound()
        {
            var result = await _service.GetAllPayrolls();
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
        #endregion

        #region GetPayrollByEmployeeId
        [Test]
        public async Task GetPayrollByEmployeeId_ShouldReturnList_WhenFound()
        {
            var payrolls = new List<Payroll>
            {
                new Payroll { Id = 1, EmployeeId = 1, PolicyId = 1, PeriodStart = DateTime.Today.AddDays(-7), PeriodEnd = DateTime.Today, StatusId = 1, NetPay = 45000 }
            };
            await _payrollRepo.AddValue(payrolls[0]);

            _mockMapper.Setup(m => m.Map<PayrollResponseDTO>(It.IsAny<Payroll>()))
                .Returns((Payroll p) => new PayrollResponseDTO
                {
                    Id = p.Id,
                    EmployeeName = "John Doe",
                    PolicyName = "Standard",
                    StatusName = "Generated",
                    NetPay = p.NetPay,
                    PeriodStart = p.PeriodStart,
                    PeriodEnd = p.PeriodEnd
                });

            var result = await _service.GetPayrollByEmployeeId(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("John Doe", result.First().EmployeeName);
        }

        [Test]
        public async Task GetPayrollByEmployeeId_ShouldReturnEmptyList_Found()
        {
            var result = await _service.GetPayrollByEmployeeId(999);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
        #endregion

        #region MarkPayrollAsPaid
        [Test]
        public async Task MarkPayrollAsPaid_ShouldReturnResponse_WhenValid()
        {
            var payroll = new Payroll
            {
                Id = 1,
                EmployeeId = 1,
                PolicyId = 1,
                PeriodStart = DateTime.Today.AddDays(-7),
                PeriodEnd = DateTime.Today,
                StatusId = 1,
                NetPay = 45000
            };
            await _payrollRepo.AddValue(payroll);

            _mockMapper.Setup(m => m.Map<PayrollResponseDTO>(It.IsAny<Payroll>()))
                .Returns((Payroll p) => new PayrollResponseDTO
                {
                    Id = p.Id,
                    EmployeeName = "John Doe",
                    PolicyName = "Standard",
                    StatusName = "Approved",
                    NetPay = p.NetPay,
                    PeriodStart = p.PeriodStart,
                    PeriodEnd = p.PeriodEnd
                });

            var result = await _service.MarkPayrollAsPaid(payroll.Id, 1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Approved", result.StatusName);
            var updatedPayroll = await _payrollRepo.GetValueById(payroll.Id);
            Assert.AreEqual(3, updatedPayroll.StatusId);
            Assert.AreEqual(1, updatedPayroll.PaidBy);
        }

        [Test]
        public void MarkPayrollAsPaid_ShouldThrowException_WhenPayrollNotFound()
        {
            Assert.ThrowsAsync<NoItemFoundException>(() => _service.MarkPayrollAsPaid(999, 1));
        }
        #endregion

        #region GenerateComplianceReport
        [Test]
        public async Task GenerateComplianceReport_ShouldReturnReport_WhenFound()
        {
            var payrolls = new List<Payroll>
            {
                new Payroll { Id = 1, EmployeeId = 1, PolicyId = 1, PeriodStart = DateTime.Today.AddDays(-7), PeriodEnd = DateTime.Today, StatusId = 3, NetPay = 45000 }
            };
            await _payrollRepo.AddValue(payrolls[0]);

            var result = await _service.GenerateComplianceReport(DateTime.Today.AddDays(-7), DateTime.Today);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.EmployeeDetails.Count);
            Assert.AreEqual("John Doe", result.EmployeeDetails.First().EmployeeName);
            Assert.AreEqual(50000, result.TotalGrossSalary);
            Assert.AreEqual(5000, result.TotalPFContribution); // 5% employee + 5% employer of 50000
        }

        [Test]
        public async Task GenerateComplianceReport_ShouldReturnEmptyReport_WhenNoneFound()
        {
            var result = await _service.GenerateComplianceReport(DateTime.Today.AddDays(-7), DateTime.Today);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.EmployeeDetails.Count);
            Assert.AreEqual(0, result.TotalGrossSalary);
            Assert.AreEqual(0, result.TotalPFContribution);
        }
        #endregion

        #region VerifyPayroll
        [Test]
        public async Task VerifyPayroll_ShouldReturnResponse_WhenValid()
        {
            var payroll = new Payroll
            {
                Id = 1,
                EmployeeId = 1,
                PolicyId = 1,
                PeriodStart = DateTime.Today.AddDays(-7),
                PeriodEnd = DateTime.Today,
                StatusId = 1,
                NetPay = 45000
            };
            await _payrollRepo.AddValue(payroll);

            _mockMapper.Setup(m => m.Map<PayrollResponseDTO>(It.IsAny<Payroll>()))
                .Returns((Payroll p) => new PayrollResponseDTO
                {
                    Id = p.Id,
                    EmployeeName = "John Doe",
                    PolicyName = "Standard",
                    StatusName = "Processed",
                    NetPay = p.NetPay,
                    PeriodStart = p.PeriodStart,
                    PeriodEnd = p.PeriodEnd
                });

            var result = await _service.VerifyPayroll(payroll.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Processed", result.StatusName);
            var updatedPayroll = await _payrollRepo.GetValueById(payroll.Id);
            Assert.AreEqual(2, updatedPayroll.StatusId);
        }

        [Test]
        public void VerifyPayroll_ShouldThrowException_WhenPayrollNotFound()
        {
            Assert.ThrowsAsync<NoItemFoundException>(() => _service.VerifyPayroll(999));
        }
        #endregion
    }
}