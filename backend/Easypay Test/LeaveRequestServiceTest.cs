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
using System.Linq;
using System.Threading.Tasks;

namespace Easypay_Test
{
    public class LeaveRequestServiceTest
    {
        private IRepository<int, Employee> _employeeRepo;
        private IRepository<int, LeaveRequest> _leaveRequestRepo;
        private IRepository<int, LeaveStatusMaster> _statusRepo;
        private IRepository<int, LeaveTypeMaster> _typeRepo;
        private Mock<IMapper> _mapper;
        private LeaveRequestService _service;
        private PayrollContext _context;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<PayrollContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new PayrollContext(options);

            _employeeRepo = new EmployeeRepository(_context);
            _leaveRequestRepo = new LeaveRequestRepository(_context);
            _statusRepo = new LeaveStatusRepository(_context);
            _typeRepo = new LeaveTypeRepository(_context);

            _employeeRepo.AddValue(new Employee { Id = 1, FirstName = "Asha", LastName = "Kumar" });
            _employeeRepo.AddValue(new Employee { Id = 2, FirstName = "Manager", LastName = "Smith" });
            _statusRepo.AddValue(new LeaveStatusMaster { Id = 1, StatusName = "Pending" });
            _statusRepo.AddValue(new LeaveStatusMaster { Id = 2, StatusName = "Approved" });
            _statusRepo.AddValue(new LeaveStatusMaster { Id = 3, StatusName = "Rejected" });
            _typeRepo.AddValue(new LeaveTypeMaster { Id = 1, LeaveTypeName = "Sick Leave" });

            _mapper = new Mock<IMapper>();
            _service = new LeaveRequestService(_employeeRepo, _leaveRequestRepo, _statusRepo, _typeRepo, _mapper.Object);
        }

        [Test]
        public async Task AddLeaveRequest()
        {
            var dto = new LeaveRequestDTO
            {
                EmployeeId = 1,
                LeaveTypeId = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                Reason = "Fever"
            };

            _mapper.Setup(m => m.Map<LeaveRequest>(dto)).Returns(new LeaveRequest
            {
                EmployeeId = dto.EmployeeId,
                LeaveTypeId = dto.LeaveTypeId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Reason = dto.Reason,
                StatusId = 1,
                RequestedAt = DateTime.Now
            });

            _mapper.Setup(m => m.Map<LeaveRequestResponseDTO>(It.IsAny<LeaveRequest>())).Returns(
                (LeaveRequest lr) => new LeaveRequestResponseDTO
                {
                    Id = lr.Id,
                    EmployeeId = lr.EmployeeId,
                    LeaveTypeId = lr.LeaveTypeId,
                    StartDate = lr.StartDate,
                    EndDate = lr.EndDate,
                    Reason = lr.Reason,
                    EmployeeName = "Asha Kumar",
                    LeaveTypeName = "Sick Leave",
                    StatusName = "Pending"
                });

            var result = await _service.SubmitLeaveRequest(dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.EmployeeName, Is.EqualTo("Asha Kumar"));
        }

        [Test]
        public async Task GetLeaveRequestById()
        {
            var leaveRequest = new LeaveRequest
            {
                Id = 1,
                EmployeeId = 1,
                LeaveTypeId = 1,
                StatusId = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                Reason = "Test",
                RequestedAt = DateTime.Now
            };

            await _leaveRequestRepo.AddValue(leaveRequest);

            _mapper.Setup(m => m.Map<LeaveRequestResponseDTO>(leaveRequest)).Returns(
                new LeaveRequestResponseDTO
                {
                    Id = leaveRequest.Id,
                    EmployeeId = leaveRequest.EmployeeId,
                    LeaveTypeId = leaveRequest.LeaveTypeId,
                    StartDate = leaveRequest.StartDate,
                    EndDate = leaveRequest.EndDate,
                    Reason = leaveRequest.Reason,
                    EmployeeName = "Asha Kumar",
                    LeaveTypeName = "Sick Leave",
                    StatusName = "Pending"
                });

            var result = await _service.GetLeaveRequestById(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.EmployeeName, Is.EqualTo("Asha Kumar"));
        }

        [Test]
        public void GetLeaveRequestById_Exception()
        {
            Assert.ThrowsAsync<NoItemFoundException>(async () => await _service.GetLeaveRequestById(999));
        }

        [Test]
        public async Task GetAllLeaveRequests()
        {
            var leaveRequest = new LeaveRequest
            {
                Id = 1,
                EmployeeId = 1,
                LeaveTypeId = 1,
                StatusId = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                Reason = "Test",
                RequestedAt = DateTime.Now
            };

            await _leaveRequestRepo.AddValue(leaveRequest);

            _mapper.Setup(m => m.Map<LeaveRequestResponseDTO>(It.IsAny<LeaveRequest>())).Returns(
                (LeaveRequest lr) => new LeaveRequestResponseDTO
                {
                    Id = lr.Id,
                    EmployeeId = lr.EmployeeId,
                    LeaveTypeId = lr.LeaveTypeId,
                    StartDate = lr.StartDate,
                    EndDate = lr.EndDate,
                    Reason = lr.Reason,
                    EmployeeName = "Asha Kumar",
                    LeaveTypeName = "Sick Leave",
                    StatusName = "Pending"
                });

            var result = await _service.GetAllLeaveRequests();

            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllLeaveRequests_Empty()
        {
            _mapper.Setup(m => m.Map<LeaveRequestResponseDTO>(It.IsAny<LeaveRequest>())).Returns(
                (LeaveRequest lr) => new LeaveRequestResponseDTO
                {
                    Id = lr.Id,
                    EmployeeId = lr.EmployeeId,
                    LeaveTypeId = lr.LeaveTypeId,
                    StartDate = lr.StartDate,
                    EndDate = lr.EndDate,
                    Reason = lr.Reason,
                    EmployeeName = "Asha Kumar",
                    LeaveTypeName = "Sick Leave",
                    StatusName = "Pending"
                });

            var result = await _service.GetAllLeaveRequests();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task ApproveLeave()
        {
            var leaveRequest = new LeaveRequest
            {
                Id = 1,
                EmployeeId = 1,
                LeaveTypeId = 1,
                StatusId = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                Reason = "Test",
                RequestedAt = DateTime.Now
            };

            await _leaveRequestRepo.AddValue(leaveRequest);

            _mapper.Setup(m => m.Map<LeaveRequestResponseDTO>(It.IsAny<LeaveRequest>())).Returns(
                (LeaveRequest lr) => new LeaveRequestResponseDTO
                {
                    Id = lr.Id,
                    EmployeeId = lr.EmployeeId,
                    LeaveTypeId = lr.LeaveTypeId,
                    StartDate = lr.StartDate,
                    EndDate = lr.EndDate,
                    Reason = lr.Reason,
                    EmployeeName = "Asha Kumar",
                    LeaveTypeName = "Sick Leave",
                    StatusName = "Approved",
                    ApprovedManagerName = "Manager Smith"
                });

            var result = await _service.ApproveLeave(1, 2, true);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusName, Is.EqualTo("Approved"));
        }

        [Test]
        public void ApproveLeave_Exception()
        {
            Assert.ThrowsAsync<NoItemFoundException>(async () => await _service.ApproveLeave(999, 2, true));
        }

        [Test]
        public async Task RejectLeave()
        {
            var leaveRequest = new LeaveRequest
            {
                Id = 1,
                EmployeeId = 1,
                LeaveTypeId = 1,
                StatusId = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                Reason = "Test",
                RequestedAt = DateTime.Now
            };

            await _leaveRequestRepo.AddValue(leaveRequest);

            _mapper.Setup(m => m.Map<LeaveRequestResponseDTO>(It.IsAny<LeaveRequest>())).Returns(
                (LeaveRequest lr) => new LeaveRequestResponseDTO
                {
                    Id = lr.Id,
                    EmployeeId = lr.EmployeeId,
                    LeaveTypeId = lr.LeaveTypeId,
                    StartDate = lr.StartDate,
                    EndDate = lr.EndDate,
                    Reason = lr.Reason,
                    EmployeeName = "Asha Kumar",
                    LeaveTypeName = "Sick Leave",
                    StatusName = "Rejected",
                    ApprovedManagerName = "Manager Smith"
                });

            var result = await _service.RejectLeave(1, 2);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusName, Is.EqualTo("Rejected"));
        }

        [Test]
        public void RejectLeave_Exception()
        {
            Assert.ThrowsAsync<NoItemFoundException>(async () => await _service.RejectLeave(999, 2));
        }

        [Test]
        public async Task DeleteLeaveRequest()
        {
            var leaveRequest = new LeaveRequest
            {
                Id = 1,
                EmployeeId = 1,
                LeaveTypeId = 1,
                StatusId = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                Reason = "Not needed",
                RequestedAt = DateTime.Now
            };

            await _leaveRequestRepo.AddValue(leaveRequest);

            _mapper.Setup(m => m.Map<LeaveRequestResponseDTO>(It.IsAny<LeaveRequest>())).Returns(
                (LeaveRequest lr) => new LeaveRequestResponseDTO
                {
                    Id = lr.Id,
                    EmployeeId = lr.EmployeeId,
                    LeaveTypeId = lr.LeaveTypeId,
                    StartDate = lr.StartDate,
                    EndDate = lr.EndDate,
                    Reason = lr.Reason,
                    EmployeeName = "Asha Kumar",
                    LeaveTypeName = "Sick Leave",
                    StatusName = "Pending"
                });

            var result = await _service.DeleteLeaveRequest(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public void DeleteLeaveRequest_Exception()
        {
            Assert.ThrowsAsync<NoItemFoundException>(async () => await _service.DeleteLeaveRequest(999));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}