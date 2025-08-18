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

namespace Easypay_Test
{
    public class PayrollPolicyServiceTest
    {
        private IRepository<int, PayrollPolicyMaster> _policyRepo;
        private PayrollPolicyService _service;
        private Mock<IMapper> _mockMapper;
        private PayrollContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PayrollContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new PayrollContext(options);
            _policyRepo = new PayrollPolicyRepository(_context);
            _mockMapper = new Mock<IMapper>();
            _service = new PayrollPolicyService(_policyRepo, _mockMapper.Object);
        }

        #region AddPolicy
        [Test]
        public async Task AddPolicy()
        {
            var dto = new PayrollPolicyAddRequestDTO
            {
                PolicyName = "Policy A",
                BasicPercent = 40,
                HRAPercent = 20,
                SpecialPercent = 10,
                TravelPercent = 5,
                MedicalPercent = 10,
                EmployeePercent = 5,
                EmployerPercent = 5,
                GratuityPercent = 5,
                TaxRegime = "Old",
                EffectiveFrom = DateTime.Now,
                EffectiveTo = DateTime.Now.AddYears(1),
                IsActive = true
            };

            var entity = new PayrollPolicyMaster { Id = 1, PolicyName = "Policy A" };

            _mockMapper.Setup(m => m.Map<PayrollPolicyMaster>(dto)).Returns(entity);
            _mockMapper.Setup(m => m.Map<PayrollPolicyAddResponseDTO>(entity))
                .Returns(new PayrollPolicyAddResponseDTO { Id = entity.Id, PolicyName = entity.PolicyName });

            var result = await _service.AddPolicy(dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.PolicyName, Is.EqualTo("Policy A"));
        }
        #endregion

        #region GetById
        [Test]
        public async Task GetById()
        {
            var entity = new PayrollPolicyMaster { Id = 1, PolicyName = "Policy X" };
            await _policyRepo.AddValue(entity);

            _mockMapper.Setup(m => m.Map<PayrollPolicyAddResponseDTO>(entity))
                .Returns(new PayrollPolicyAddResponseDTO { Id = entity.Id, PolicyName = entity.PolicyName });

            var result = await _service.GetById(1);

            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.PolicyName, Is.EqualTo("Policy X"));
        }

        [Test]
        public void GetById_ThrowException()
        {
            Assert.ThrowsAsync<NoItemFoundException>(async () => await _service.GetById(999));
        }
        #endregion

        #region GetAll
        [Test]
        public async Task GetAll()
        {
            await _policyRepo.AddValue(new PayrollPolicyMaster { Id = 1, PolicyName = "One" });
            await _policyRepo.AddValue(new PayrollPolicyMaster { Id = 2, PolicyName = "Two" });

            _mockMapper.Setup(m => m.Map<PayrollPolicyAddResponseDTO>(It.IsAny<PayrollPolicyMaster>()))
                .Returns<PayrollPolicyMaster>(p => new PayrollPolicyAddResponseDTO { Id = p.Id, PolicyName = p.PolicyName });

            var result = await _service.GetAll();

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetAll_ThrowException()
        {
            Assert.ThrowsAsync<NoItemFoundException>(async () => await _service.GetAll());
        }
        #endregion

        #region UpdatePolicy
        [Test]
        public async Task UpdatePolicy()
        {
            var original = new PayrollPolicyMaster
            {
                Id = 1,
                PolicyName = "Original",
                BasicPercent = 30,
                HRAPercent = 10,
                SpecialPercent = 5,
                TravelPercent = 5,
                MedicalPercent = 5,
                EmployeePercent = 5,
                EmployerPercent = 5,
                GratuityPercent = 5,
                TaxRegime = "Old",
                EffectiveFrom = DateTime.Now,
                EffectiveTo = DateTime.Now.AddYears(1),
                IsActive = true
            };
            await _policyRepo.AddValue(original);

            var updateDto = new PayrollPolicyAddRequestDTO
            {
                PolicyName = "Updated",
                BasicPercent = 40,
                HRAPercent = 20,
                SpecialPercent = 10,
                TravelPercent = 5,
                MedicalPercent = 5,
                EmployeePercent = 5,
                EmployerPercent = 5,
                GratuityPercent = 5,
                TaxRegime = "New",
                EffectiveFrom = DateTime.Now,
                EffectiveTo = DateTime.Now.AddYears(1),
                IsActive = true
            };

            _mockMapper.Setup(m => m.Map<PayrollPolicyAddResponseDTO>(It.IsAny<PayrollPolicyMaster>()))
                .Returns<PayrollPolicyMaster>(p => new PayrollPolicyAddResponseDTO { Id = p.Id, PolicyName = p.PolicyName });

            var result = await _service.UpdatePolicy(1, updateDto);

            Assert.That(result.PolicyName, Is.EqualTo("Updated"));
        }

        [Test]
        public void UpdatePolicy_ThrowException()
        {
            var dto = new PayrollPolicyAddRequestDTO();
            Assert.ThrowsAsync<NoItemFoundException>(() => _service.UpdatePolicy(999, dto));
        }
        #endregion

        #region DeletePolicy
        [Test]
        public async Task DeletePolicy()
        {
            var entity = new PayrollPolicyMaster { Id = 1, PolicyName = "DeleteMe" };
            await _policyRepo.AddValue(entity);

            _mockMapper.Setup(m => m.Map<PayrollPolicyAddResponseDTO>(entity))
                .Returns(new PayrollPolicyAddResponseDTO { Id = entity.Id, PolicyName = entity.PolicyName });

            var result = await _service.DeletePolicy(1);

            Assert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public void DeletePolicy_ThrowException()
        {
            Assert.ThrowsAsync<NoItemFoundException>(() => _service.DeletePolicy(999));
        }
        #endregion

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
