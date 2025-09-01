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
        private Mock<IRepository<string, UserAccount>> _userRepoMock;
        private Mock<IRepository<int, AuditTrailActionMaster>> _actionRepoMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void Setup()
        {
            _auditRepoMock = new Mock<IRepository<int, AuditTrail>>();
            _userRepoMock = new Mock<IRepository<string, UserAccount>>();
            _actionRepoMock = new Mock<IRepository<int, AuditTrailActionMaster>>();
            _mapperMock = new Mock<IMapper>();

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
        public async Task LogAction_ShouldLogActionSuccessfully()
        {
            var userName = "admin";
            var actionId = 1;
            var entityName = "Employee";
            var entityId = 100;
            var oldValue = new { };
            var newValue = new { Name = "John" };
            var ipAddress = "127.0.0.1";

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
                TimeStamp = DateTime.UtcNow
            };

            _userRepoMock.Setup(repo => repo.GetValueById("admin"))
                .ReturnsAsync(new UserAccount { Id = 1, UserName = "admin" });
            _actionRepoMock.Setup(repo => repo.GetValueById(1))
                .ReturnsAsync(new AuditTrailActionMaster { Id = 1, ActionName = "Create" });
            _auditRepoMock.Setup(repo => repo.AddValue(It.IsAny<AuditTrail>()))
                .ReturnsAsync(auditTrail);

            await _service.LogAction(userName, actionId, entityName, entityId, oldValue, newValue, ipAddress);

            _auditRepoMock.Verify(repo => repo.AddValue(It.Is<AuditTrail>(a =>
                a.UserId == 1 &&
                a.UserName == "admin" &&
                a.ActionId == 1 &&
                a.EntityName == "Employee" &&
                a.EntityId == 100 &&
                a.OldValue == "{}" &&
                a.NewValue == "{\"Name\":\"John\"}" &&
                a.IPAddress == "127.0.0.1")), Times.Once());
        }

        [Test]
        public void LogAction_UserNotFound_ShouldThrowNoItemFoundException()
        {
            var userName = "unknown";
            var actionId = 1;
            var entityName = "Employee";
            var entityId = 100;
            var oldValue = new { };
            var newValue = new { Name = "John" };
            var ipAddress = "127.0.0.1";

            Assert.ThrowsAsync<NoItemFoundException>(() =>
                _service.LogAction(userName, actionId, entityName, entityId, oldValue, newValue, ipAddress));
        }

        [Test]
        public void LogAction_ActionNotFound_ShouldThrowNoItemFoundException()
        {
            var userName = "admin";
            var actionId = 999;
            var entityName = "Employee";
            var entityId = 100;
            var oldValue = new { };
            var newValue = new { Name = "John" };
            var ipAddress = "127.0.0.1";

            _userRepoMock.Setup(repo => repo.GetValueById("admin"))
                .ReturnsAsync(new UserAccount { Id = 1, UserName = "admin" });

            Assert.ThrowsAsync<NoItemFoundException>(() =>
                _service.LogAction(userName, actionId, entityName, entityId, oldValue, newValue, ipAddress));
        }
        #endregion

        #region GetAllLogs
        [Test]
        public async Task GetAllLogs_ShouldReturnAllLogs()
        {
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

            var result = await _service.GetAllLogs();

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

            var result = await _service.GetLogsByUser("admin");

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

            var result = await _service.GetLogsByAction(1);

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

            var result = await _service.GetLogById(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.EntityName, Is.EqualTo("Employee"));
            Assert.That(result.UserName, Is.EqualTo("admin"));
            Assert.That(result.ActionName, Is.EqualTo("Create"));
        }

        [Test]
        public void GetLogById_NotFound_ShouldThrow()
        {
            _auditRepoMock.Setup(repo => repo.GetValueById(999))
                .ReturnsAsync((AuditTrail)null);

            Assert.ThrowsAsync<NoItemFoundException>(() => _service.GetLogById(999));
        }
        #endregion
    }
}