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

namespace Easypay_Test
{
    public class AuditTrailServiceTest
    {
        private IAuditTrailService _service;
        private Mock<IRepository<int, AuditTrail>> _auditRepoMock;
        private Mock<IRepository<string, UserAccount>> _userRepoMock; // Changed to string
        private Mock<IRepository<int, AuditTrailActionMaster>> _actionRepoMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void Setup()
        {
            _auditRepoMock = new Mock<IRepository<int, AuditTrail>>();
            _userRepoMock = new Mock<IRepository<string, UserAccount>>(); // Changed to string
            _actionRepoMock = new Mock<IRepository<int, AuditTrailActionMaster>>();
            _mapperMock = new Mock<IMapper>();

            // Setup mock data for repositories
            _userRepoMock.Setup(repo => repo.GetValueById(It.IsAny<string>()))
                .ReturnsAsync((string userName) => userName.ToLower() == "admin" ? new UserAccount { Id = 1, UserName = "admin" } : null);
            _actionRepoMock.Setup(repo => repo.GetValueById(It.IsAny<int>()))
                .ReturnsAsync((int id) => id == 1 ? new AuditTrailActionMaster { Id = 1, ActionName = "Create" } : null);

            _service = new AuditTrailService(
                _auditRepoMock.Object,
                _userRepoMock.Object,
                _actionRepoMock.Object,
                _mapperMock.Object
            );
        }

        #region LogAction
        [Test]
        public async Task LogAction()
        {
            // Arrange
            var request = new AuditTrailRequestDTO
            {
                UserName = "admin", // Changed to UserName
                ActionId = 1,
                EntityName = "Employee",
                EntityId = 100,
                OldValue = "{}",
                NewValue = "{\"Name\":\"John\"}",
                IPAddress = "127.0.0.1"
            };

            var auditTrail = new AuditTrail
            {
                Id = 1,
                UserId = 1,
                UserName = "admin",
                ActionId = 1,
                EntityName = "Employee",
                EntityId = 100,
                OldValue = "{}",
                NewValue = "{\"Name\":\"John\"}",
                IPAddress = "127.0.0.1",
                TimeStamp = DateTime.Now
            };

            var response = new AuditTrailResponseDTO
            {
                Id = 1,
                UserId = 1,
                UserName = "admin",
                ActionId = 1,
                ActionName = "Create",
                EntityName = "Employee",
                EntityId = 100,
                OldValue = "{}",
                NewValue = "{\"Name\":\"John\"}",
                IPAddress = "127.0.0.1",
                TimeStamp = auditTrail.TimeStamp
            };

            _mapperMock.Setup(m => m.Map<AuditTrail>(It.IsAny<AuditTrailRequestDTO>()))
                .Returns(auditTrail);
            _auditRepoMock.Setup(repo => repo.AddValue(It.IsAny<AuditTrail>()))
                .ReturnsAsync(auditTrail);
            _mapperMock.Setup(m => m.Map<AuditTrailResponseDTO>(It.IsAny<AuditTrail>()))
                .Returns(response);

            // Act
            var result = await _service.LogAction(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionId, Is.EqualTo(1));
            Assert.That(result.UserName, Is.EqualTo("admin"));
            Assert.That(result.ActionName, Is.EqualTo("Create"));
            Assert.That(result.EntityName, Is.EqualTo("Employee"));
            Assert.That(result.EntityId, Is.EqualTo(100));
            Assert.That(result.IPAddress, Is.EqualTo("127.0.0.1"));
        }
        #endregion

        #region GetAllLogs
        [Test]
        public async Task GetAllLogs_ShouldReturnAllLogs()
        {
            // Arrange
            var auditTrails = new List<AuditTrail>
            {
                new AuditTrail
                {
                    Id = 1,
                    UserId = 1,
                    UserName = "admin",
                    ActionId = 1,
                    EntityName = "Employee",
                    EntityId = 1000,
                    OldValue = "{}",
                    NewValue = "{\"Name\":\"New\"}",
                    TimeStamp = DateTime.Now,
                    IPAddress = "192.168.0.1"
                }
            };

            var response = new List<AuditTrailResponseDTO>
            {
                new AuditTrailResponseDTO
                {
                    Id = 1,
                    UserId = 1,
                    UserName = "admin",
                    ActionId = 1,
                    ActionName = "Create",
                    EntityName = "Employee",
                    EntityId = 1000,
                    OldValue = "{}",
                    NewValue = "{\"Name\":\"New\"}",
                    TimeStamp = auditTrails[0].TimeStamp,
                    IPAddress = "192.168.0.1"
                }
            };

            _auditRepoMock.Setup(repo => repo.GetAllValue())
                .ReturnsAsync(auditTrails);
            _mapperMock.Setup(m => m.Map<AuditTrailResponseDTO>(It.IsAny<AuditTrail>()))
                .Returns((AuditTrail log) => response.First(r => r.Id == log.Id));
            _userRepoMock.Setup(repo => repo.GetValueById("admin"))
                .ReturnsAsync(new UserAccount { Id = 1, UserName = "admin" });

            // Act
            var result = await _service.GetAllLogs();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().UserId, Is.EqualTo(1));
            Assert.That(result.First().UserName, Is.EqualTo("admin"));
            Assert.That(result.First().ActionName, Is.EqualTo("Create"));
        }
        #endregion

        #region GetLogsByUser
        [Test]
        public async Task GetLogsByUser_ShouldReturnCorrectLogs()
        {
            // Arrange
            var auditTrails = new List<AuditTrail>
            {
                new AuditTrail { Id = 1, UserId = 1, UserName = "admin", ActionId = 1, EntityName = "Employee", EntityId = 101, OldValue = "{}", NewValue = "{}", TimeStamp = DateTime.Now, IPAddress = "x" },
                new AuditTrail { Id = 2, UserId = 2, UserName = "user", ActionId = 1, EntityName = "Employee", EntityId = 102, OldValue = "{}", NewValue = "{}", TimeStamp = DateTime.Now, IPAddress = "x" }
            };

            var response = new List<AuditTrailResponseDTO>
            {
                new AuditTrailResponseDTO { Id = 1, UserId = 1, UserName = "admin", ActionId = 1, ActionName = "Create" }
            };

            _auditRepoMock.Setup(repo => repo.GetAllValue())
                .ReturnsAsync(auditTrails);
            _mapperMock.Setup(m => m.Map<AuditTrailResponseDTO>(It.IsAny<AuditTrail>()))
                .Returns((AuditTrail log) => new AuditTrailResponseDTO
                {
                    Id = log.Id,
                    UserId = log.UserId,
                    UserName = log.UserName,
                    ActionId = log.ActionId,
                    ActionName = log.ActionId == 1 ? "Create" : null
                });
            _userRepoMock.Setup(repo => repo.GetValueById("admin"))
                .ReturnsAsync(new UserAccount { Id = 1, UserName = "admin" });

            // Act
            var result = await _service.GetLogsByUser("admin");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(1));
            Assert.That(result.First().UserName, Is.EqualTo("admin"));
        }
        #endregion

        #region GetLogsByAction
        [Test]
        public async Task GetLogsByAction_ShouldReturnCorrectLogs()
        {
            // Arrange
            var auditTrails = new List<AuditTrail>
            {
                new AuditTrail { Id = 1, UserId = 1, UserName = "admin", ActionId = 1, EntityName = "X", EntityId = 1, OldValue = "", NewValue = "", TimeStamp = DateTime.Now, IPAddress = "" },
                new AuditTrail { Id = 2, UserId = 2, UserName = "user", ActionId = 2, EntityName = "X", EntityId = 2, OldValue = "", NewValue = "", TimeStamp = DateTime.Now, IPAddress = "" }
            };

            var response = new List<AuditTrailResponseDTO>
            {
                new AuditTrailResponseDTO { Id = 1, UserId = 1, UserName = "admin", ActionId = 1, ActionName = "Create" }
            };

            _auditRepoMock.Setup(repo => repo.GetAllValue())
                .ReturnsAsync(auditTrails);
            _mapperMock.Setup(m => m.Map<AuditTrailResponseDTO>(It.IsAny<AuditTrail>()))
                .Returns((AuditTrail log) => new AuditTrailResponseDTO
                {
                    Id = log.Id,
                    UserId = log.UserId,
                    UserName = log.UserName,
                    ActionId = log.ActionId,
                    ActionName = log.ActionId == 1 ? "Create" : null
                });
            _userRepoMock.Setup(repo => repo.GetValueById("admin"))
                .ReturnsAsync(new UserAccount { Id = 1, UserName = "admin" });

            // Act
            var result = await _service.GetLogsByAction(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().ActionId, Is.EqualTo(1));
            Assert.That(result.First().UserName, Is.EqualTo("admin"));
            Assert.That(result.First().ActionName, Is.EqualTo("Create"));
        }
        #endregion

        #region GetLogById
        [Test]
        public async Task GetLogById_ShouldReturnCorrectLog()
        {
            // Arrange
            var auditTrail = new AuditTrail
            {
                Id = 1,
                UserId = 1,
                UserName = "admin",
                ActionId = 1,
                EntityName = "Employee",
                EntityId = 123,
                OldValue = "{}",
                NewValue = "{}",
                TimeStamp = DateTime.Now,
                IPAddress = "127.0.0.1"
            };

            var response = new AuditTrailResponseDTO
            {
                Id = 1,
                UserId = 1,
                UserName = "admin",
                ActionId = 1,
                ActionName = "Create",
                EntityName = "Employee",
                EntityId = 123,
                OldValue = "{}",
                NewValue = "{}",
                TimeStamp = auditTrail.TimeStamp,
                IPAddress = "127.0.0.1"
            };

            _auditRepoMock.Setup(repo => repo.GetValueById(1))
                .ReturnsAsync(auditTrail);
            _mapperMock.Setup(m => m.Map<AuditTrailResponseDTO>(It.IsAny<AuditTrail>()))
                .Returns(response);
            _userRepoMock.Setup(repo => repo.GetValueById("admin"))
                .ReturnsAsync(new UserAccount { Id = 1, UserName = "admin" });

            // Act
            var result = await _service.GetLogById(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.EntityName, Is.EqualTo("Employee"));
            Assert.That(result.UserName, Is.EqualTo("admin"));
            Assert.That(result.ActionName, Is.EqualTo("Create"));
        }

        [Test]
        public void GetLogById_NotFound_ShouldThrow()
        {
            // Arrange
            _auditRepoMock.Setup(repo => repo.GetValueById(999))
                .ReturnsAsync((AuditTrail)null);

            // Act & Assert
            Assert.ThrowsAsync<NoItemFoundException>(() => _service.GetLogById(999));
        }
        #endregion
    }
}