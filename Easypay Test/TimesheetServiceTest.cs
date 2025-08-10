using AutoMapper;
using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;
using Easypay_App.Repositories;
using Easypay_App.Services;
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
    public class TimesheetServiceTest
    {
        private IRepository<int, Timesheet> _timesheetRepo;
        private IRepository<int, Employee> _employeeRepo;
        private IRepository<int, TimesheetStatusMaster> _statusRepo;
        private Mock<IMapper> _mockMapper;
        private TimesheetService _service;
        private PayrollContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PayrollContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new PayrollContext(options);

            _timesheetRepo = new TimesheetRepository(_context);
            _employeeRepo = new EmployeeRepository(_context);
            _statusRepo = new TimesheetStatusRepository(_context);

            _mockMapper = new Mock<IMapper>();
            _service = new TimesheetService(_timesheetRepo, _employeeRepo, _statusRepo, _mockMapper.Object);

            // Seed data directly in Setup
            var employees = new List<Employee>
            {
                new Employee { Id = 1, FirstName = "John", LastName = "Doe" },
                new Employee { Id = 2, FirstName = "Jane", LastName = "Smith" }
            };
            _context.Employees.AddRange(employees);

            var statuses = new List<TimesheetStatusMaster>
            {
                new TimesheetStatusMaster { Id = 1, StatusName = "Pending" },
                new TimesheetStatusMaster { Id = 2, StatusName = "Approved" },
                new TimesheetStatusMaster { Id = 3, StatusName = "Rejected" }
            };
            _context.TimesheetStatusMasters.AddRange(statuses);

            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        #region AddTimesheet
        [Test]
        public async Task AddTimesheet_ShouldReturnResponse_WhenValid()
        {
            var requestDto = new TimesheetRequestDTO
            {
                EmployeeId = 1,
                WorkDate = DateTime.Today,
                HoursWorked = 8,
                TaskDescription = "Test Task",
                IsBillable = true
            };

            var timesheet = new Timesheet
            {
                Id = 1,
                EmployeeId = requestDto.EmployeeId,
                WorkDate = requestDto.WorkDate,
                HoursWorked = requestDto.HoursWorked,
                TaskDescription = requestDto.TaskDescription,
                IsBillable = requestDto.IsBillable,
                StatusId = 1,
                CreatedAt = DateTime.Now
            };

            _mockMapper.Setup(m => m.Map<Timesheet>(requestDto)).Returns(timesheet);
            _mockMapper.Setup(m => m.Map<TimesheetResponseDTO>(It.IsAny<Timesheet>()))
                .Returns((Timesheet t) => new TimesheetResponseDTO
                {
                    Id = t.Id,
                    EmployeeName = "John Doe",
                    WorkDate = t.WorkDate,
                    HoursWorked = t.HoursWorked,
                    TaskDescription = t.TaskDescription,
                    IsBillable = t.IsBillable,
                    StatusName = "Pending"
                });

            var result = await _service.AddTimesheet(requestDto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.HoursWorked, Is.EqualTo(8));
            Assert.That(result.StatusName, Is.EqualTo("Pending"));
            Assert.That(result.EmployeeName, Is.EqualTo("John Doe"));
        }

        [Test]
        public void AddTimesheet_ShouldThrowException_WhenEmployeeNotFound()
        {
            var requestDto = new TimesheetRequestDTO
            {
                EmployeeId = 999,
                WorkDate = DateTime.Today,
                HoursWorked = 8
            };

            Assert.ThrowsAsync<NoItemFoundException>(() => _service.AddTimesheet(requestDto));
        }
        #endregion

        #region ApproveTimesheet
        [Test]
        public async Task ApproveTimesheet_ShouldReturnTrue_WhenValid()
        {
            var timesheet = new Timesheet
            {
                Id = 1,
                EmployeeId = 1,
                WorkDate = DateTime.Today,
                HoursWorked = 8,
                StatusId = 1
            };
            await _timesheetRepo.AddValue(timesheet);

            var result = await _service.ApproveTimesheet(timesheet.Id);

            Assert.That(result, Is.True);
            var updatedTimesheet = await _timesheetRepo.GetValueById(timesheet.Id);
            Assert.That(updatedTimesheet.StatusId, Is.EqualTo(2));
        }

        [Test]
        public void ApproveTimesheet_ShouldThrowException_WhenTimesheetNotFound()
        {
            Assert.ThrowsAsync<NoItemFoundException>(() => _service.ApproveTimesheet(999));
        }
        #endregion

        #region RejectTimesheet
        [Test]
        public async Task RejectTimesheet_ShouldReturnTrue_WhenValid()
        {
            var timesheet = new Timesheet
            {
                Id = 1,
                EmployeeId = 1,
                WorkDate = DateTime.Today,
                HoursWorked = 8,
                StatusId = 1
            };
            await _timesheetRepo.AddValue(timesheet);

            var result = await _service.RejectTimesheet(timesheet.Id);

            Assert.That(result, Is.True);
            var updatedTimesheet = await _timesheetRepo.GetValueById(timesheet.Id);
            Assert.That(updatedTimesheet.StatusId, Is.EqualTo(3));
        }

        [Test]
        public void RejectTimesheet_ShouldThrowException_WhenTimesheetNotFound()
        {
            Assert.ThrowsAsync<NoItemFoundException>(() => _service.RejectTimesheet(999));
        }
        #endregion

        #region GetTimesheetsByDateRange
        [Test]
        public async Task GetTimesheetsByDateRange_ShouldReturnList_WhenFound()
        {
            var timesheets = new List<Timesheet>
            {
                new Timesheet { Id = 1, EmployeeId = 1, WorkDate = DateTime.Today, HoursWorked = 8, StatusId = 1 },
                new Timesheet { Id = 2, EmployeeId = 2, WorkDate = DateTime.Today.AddDays(-1), HoursWorked = 7, StatusId = 1 }
            };
            foreach (var ts in timesheets)
            {
                await _timesheetRepo.AddValue(ts);
            }

            _mockMapper.Setup(m => m.Map<TimesheetResponseDTO>(It.IsAny<Timesheet>()))
                .Returns((Timesheet t) => new TimesheetResponseDTO
                {
                    Id = t.Id,
                    EmployeeName = t.EmployeeId == 1 ? "John Doe" : "Jane Smith",
                    WorkDate = t.WorkDate,
                    HoursWorked = t.HoursWorked,
                    StatusName = "Pending"
                });

            var result = await _service.GetTimesheetsByDateRange(DateTime.Today.AddDays(-1), DateTime.Today);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.Any(r => r.EmployeeName == "Jane Smith" && r.HoursWorked == 7), Is.True, "Timesheet for Jane Smith with 7 hours should exist");
            Assert.That(result.Any(r => r.EmployeeName == "John Doe" && r.HoursWorked == 8), Is.True, "Timesheet for John Doe with 8 hours should exist");
        }

        [Test]
        public void GetTimesheetsByDateRange_ShouldThrowException_WhenNoneFound()
        {
            Assert.ThrowsAsync<NoItemFoundException>(() =>
                _service.GetTimesheetsByDateRange(DateTime.Today.AddDays(-1), DateTime.Today));
        }
        #endregion

        #region GetTimesheetsByEmployee
        [Test]
        public async Task GetTimesheetsByEmployee_ShouldReturnList_WhenFound()
        {
            var timesheets = new List<Timesheet>
            {
                new Timesheet { Id = 1, EmployeeId = 1, WorkDate = DateTime.Today, HoursWorked = 8, StatusId = 1 }
            };
            await _timesheetRepo.AddValue(timesheets[0]);

            _mockMapper.Setup(m => m.Map<TimesheetResponseDTO>(It.IsAny<Timesheet>()))
                .Returns((Timesheet t) => new TimesheetResponseDTO
                {
                    Id = t.Id,
                    EmployeeName = "John Doe",
                    WorkDate = t.WorkDate,
                    HoursWorked = t.HoursWorked,
                    StatusName = "Pending"
                });

            var result = await _service.GetTimesheetsByEmployee(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().EmployeeName, Is.EqualTo("John Doe"));
        }

        [Test]
        public void GetTimesheetsByEmployee_ShouldThrowException_WhenNoneFound()
        {
            Assert.ThrowsAsync<NoItemFoundException>(() => _service.GetTimesheetsByEmployee(999));
        }
        #endregion
    }
}