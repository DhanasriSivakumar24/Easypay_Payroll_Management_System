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
    public class BenefitEnrollmentServiceTest
    {
        private IRepository<int, BenefitMaster> _benefitRepo;
        private IRepository<int, BenefitEnrollment> _enrollmentRepo;
        private IRepository<int, BenefitStatusMaster> _statusRepo;
        private IRepository<int, Employee> _employeeRepo;
        private Mock<IMapper> _mockMapper;
        private Mock<IAuditTrailService> _mockAuditTrailService;
        private BenefitEnrollmentService _service;
        private PayrollContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PayrollContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new PayrollContext(options);

            _benefitRepo = new BenefitsRepository(_context);
            _enrollmentRepo = new BenefitEnrollmentRepository(_context);
            _statusRepo = new BenefitsStatusRepository(_context);
            _employeeRepo = new EmployeeRepository(_context);

            _mockMapper = new Mock<IMapper>();
            _mockAuditTrailService = new Mock<IAuditTrailService>();

            // Seed data
            _benefitRepo.AddValue(new BenefitMaster { Id = 1, BenefitName = "Test Benefit", EmployeeContribution = 100, EmployerContribution = 80 });
            _statusRepo.AddValue(new BenefitStatusMaster { Id = 1, StatusName = "Pending" });
            _employeeRepo.AddValue(new Employee { Id = 1, FirstName = "John", LastName = "Doe" });

            _service = new BenefitEnrollmentService(_benefitRepo, _enrollmentRepo, _statusRepo, _mockAuditTrailService.Object, _mockMapper.Object);
        }

        #region EnrollBenefit
        [Test]
        public async Task EnrollBenefit()
        {
            var dto = new BenefitEnrollmentAddRequestDTO
            {
                EmployeeId = 1,
                BenefitId = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                StatusId = 1
            };

            var entity = new BenefitEnrollment
            {
                Id = 99,
                EmployeeId = dto.EmployeeId,
                BenefitId = dto.BenefitId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                StatusId = dto.StatusId
            };

            var expected = new BenefitEnrollmentAddResponseDTO
            {
                Id = 99,
                BenefitId = 1,
                EmployeeId = 1,
                BenefitName = "Test Benefit",
                StatusName = "Pending",
                EmployeeName = "John Doe"
            };

            _mockMapper.Setup(m => m.Map<BenefitEnrollment>(dto)).Returns(entity);
            _mockMapper.Setup(m => m.Map<BenefitEnrollmentAddResponseDTO>(It.IsAny<BenefitEnrollment>())).Returns(expected);

            var result = await _service.EnrollBenefit(dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.EmployeeId, Is.EqualTo(1));
            Assert.That(result.BenefitName, Is.EqualTo("Test Benefit"));
        }
        #endregion

        #region GetAll
        [Test]
        public async Task GetAllEnrollments()
        {
            var entity = new BenefitEnrollment
            {
                EmployeeId = 1,
                BenefitId = 1,
                StatusId = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                CreatedAt = DateTime.Now
            };
            await _enrollmentRepo.AddValue(entity);

            _mockMapper.Setup(m => m.Map<BenefitEnrollmentAddResponseDTO>(It.IsAny<BenefitEnrollment>()))
                .Returns(new BenefitEnrollmentAddResponseDTO { Id = entity.Id, EmployeeId = 1, BenefitName = "Test Benefit", StatusName = "Pending" });

            var result = await _service.GetAllBenefit();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.GreaterThan(0));
        }

        [Test]
        public void GetAll_ThrowException()
        {
            Assert.ThrowsAsync<NoItemFoundException>(() => _service.GetAllBenefit());
        }
        #endregion

        #region GetById
        [Test]
        public async Task GetById()
        {
            var entity = new BenefitEnrollment
            {
                EmployeeId = 1,
                BenefitId = 1,
                StatusId = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                CreatedAt = DateTime.Now
            };
            await _enrollmentRepo.AddValue(entity);

            _mockMapper.Setup(m => m.Map<BenefitEnrollmentAddResponseDTO>(It.IsAny<BenefitEnrollment>()))
                .Returns(new BenefitEnrollmentAddResponseDTO { Id = entity.Id, EmployeeId = 1, BenefitName = "Test Benefit", StatusName = "Pending" });

            var result = await _service.GetBenefitById(entity.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(entity.Id));
        }

        [Test]
        public void GetById_ThrowException()
        {
            Assert.ThrowsAsync<NoItemFoundException>(() => _service.GetBenefitById(999));
        }
        #endregion

        #region Update
        [Test]
        public async Task Update()
        {
            var entity = new BenefitEnrollment
            {
                EmployeeId = 1,
                BenefitId = 1,
                StatusId = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                CreatedAt = DateTime.Now
            };
            await _enrollmentRepo.AddValue(entity);

            var dto = new BenefitEnrollmentAddRequestDTO
            {
                EmployeeId = 1,
                BenefitId = 1,
                StatusId = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(2)
            };

            _mockMapper.Setup(m => m.Map<BenefitEnrollmentAddResponseDTO>(It.IsAny<BenefitEnrollment>()))
                .Returns(new BenefitEnrollmentAddResponseDTO { Id = entity.Id, BenefitName = "Test Benefit", StatusName = "Pending" });

            var result = await _service.UpdateBenefit(entity.Id, dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(entity.Id));
        }

        [Test]
        public void Update_ThrowException()
        {
            var dto = new BenefitEnrollmentAddRequestDTO { EmployeeId = 1, BenefitId = 1, StatusId = 1 };
            Assert.ThrowsAsync<NoItemFoundException>(() => _service.UpdateBenefit(999, dto));
        }
        #endregion

        #region Delete
        [Test]
        public async Task Delete()
        {
            var entity = new BenefitEnrollment
            {
                EmployeeId = 1,
                BenefitId = 1,
                StatusId = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                CreatedAt = DateTime.Now
            };
            await _enrollmentRepo.AddValue(entity);

            _mockMapper.Setup(m => m.Map<BenefitEnrollmentAddResponseDTO>(It.IsAny<BenefitEnrollment>()))
                .Returns(new BenefitEnrollmentAddResponseDTO { Id = entity.Id, BenefitName = "Test Benefit", StatusName = "Pending" });

            var result = await _service.DeleteBenefit(entity.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(entity.Id));
        }

        [Test]
        public void Delete_ThrowException()
        {
            Assert.ThrowsAsync<NoItemFoundException>(() => _service.DeleteBenefit(999));
        }
        #endregion

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}