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
    public class NotificationLogServiceTest
    {
        private PayrollContext _context;
        private IMapper _mapper;
        private IRepository<int, NotificationLog> _notificationRepo;
        private NotificationLogService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PayrollContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new PayrollContext(options);

            _notificationRepo = new NotificationRepository(_context);

            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new Easypay_App.Mapper.EmployeeMapper()));
            _mapper = mockMapper.CreateMapper();

            _service = new NotificationLogService(_context, _notificationRepo, _mapper);

            _context.NotificationChannelMasters.Add(new NotificationChannelMaster { Id = 1, Name = "Email" });
            _context.NotificationStatusMasters.Add(new NotificationStatusMaster { Id = 2, StatusName = "Sent" });

            _context.UserAccounts.Add(new UserAccount
            {
                Id = 1,
                UserName = "testuser",
                EmployeeId = 1,
                UserRoleId = 1,
                ActiveFlag = true,
                LastLogin = DateTime.Now,
                Password = new byte[] { 1, 2, 3 },
                HashKey = new byte[] { 4, 5, 6 }
            });

            _context.SaveChanges();
        }

        #region SendNotification
        [Test]
        public async Task SendNotification()
        {
            var request = new NotificationLogRequestDTO
            {
                UserId = 1,
                ChannelId = 1,
                Message = "Test message"
            };

            var result = await _service.SendNotification(request);

            Assert.IsNotNull(result);
            Assert.That(result.Message, Is.EqualTo("Test message"));
            Assert.That(result.ChannelName, Is.EqualTo("Email"));
            Assert.That(result.Status, Is.EqualTo("Sent"));
        }

        [Test]
        public void SendNotification_EmptyMessage_ThrowException()
        {
            var request = new NotificationLogRequestDTO
            {
                UserId = 1,
                ChannelId = 1,
                Message = ""
            };

            Assert.ThrowsAsync<InvalidDataException>(() => _service.SendNotification(request));
        }

        [Test]
        public void SendNotification_InvalidUser_ThrowException()
        {
            var request = new NotificationLogRequestDTO
            {
                UserId = 999,
                ChannelId = 1,
                Message = "Hello"
            };

            Assert.ThrowsAsync<NoItemFoundException>(() => _service.SendNotification(request));
        }

        [Test]
        public void SendNotification_InvalidChannel_ThrowException()
        {
            var request = new NotificationLogRequestDTO
            {
                UserId = 1,
                ChannelId = 999,
                Message = "Hello"
            };

            Assert.ThrowsAsync<NoItemFoundException>(() => _service.SendNotification(request));
        }
        #endregion

        #region GetNotificationsByUser
        [Test]
        public async Task GetNotificationsByUser()
        {
            _context.NotificationLogs.Add(new NotificationLog
            {
                Id = 1,
                UserId = 1,
                ChannelId = 1,
                Message = "Reminder",
                SendDate = DateTime.Now,
                StatusId = 2,
                StatusMessage = "Sent"
            });
            _context.SaveChanges();

            var result = await _service.GetNotificationsByUser(1);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Message, Is.EqualTo("Reminder"));
        }

        [Test]
        public void GetNotificationsByUser_InvalidUser_ThrowException()
        {
            Assert.ThrowsAsync<NoItemFoundException>(() => _service.GetNotificationsByUser(999));
        }
        #endregion
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
