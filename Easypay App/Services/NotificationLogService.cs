using AutoMapper;
using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Easypay_App.Services
{
    public class NotificationLogService : INotificationLogService
    {
        private readonly PayrollContext _context;
        private readonly IRepository<int, NotificationLog> _notificationRepository;
        private readonly IMapper _mapper;

        public NotificationLogService(PayrollContext context,
            IRepository<int, NotificationLog> notificationRepository,
            IMapper mapper)
        {
            _context = context;
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public IEnumerable<NotificationLogDTO> GetNotificationsByUser(int userId)
        {
            var userExists = _context.UserAccounts.Any(u => u.Id == userId);
            if (!userExists)
                throw new NoItemFoundException("User not found");

            var logs = _context.NotificationLogs
                .Include(n => n.Channel)
                .Include(n => n.Status)
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.SendDate)
                .ToList();

            var dtoList = logs.Select(log => new NotificationLogDTO
            {
                Id = log.Id,
                UserId = log.UserId,
                ChannelName = log.Channel?.Name ?? "Unknown",
                Message = log.Message,
                SendDate = log.SendDate,
                Status = log.Status?.StatusName ?? "Unknown"
            });

            return dtoList;
        }

        public NotificationLogDTO SendNotification(NotificationLogRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                throw new InvalidDataException("Message cannot be empty");

            // Validate user
            var user = _context.UserAccounts.FirstOrDefault(u => u.Id == request.UserId);
            if (user == null)
                throw new NoItemFoundException("User not found");

            // Validate channel
            var channel = _context.NotificationChannelMasters.FirstOrDefault(c => c.Id == request.ChannelId);
            if (channel == null)
                throw new NoItemFoundException("Notification channel not found");

            var notification = new NotificationLog
            {
                UserId = request.UserId,
                ChannelId = request.ChannelId,
                Message = request.Message,
                SendDate = DateTime.Now,
                StatusId = 2, // default to "Sent"
                StatusMessage = "Notification sent successfully"
            };

            _notificationRepository.AddValue(notification);

            var dto = new NotificationLogDTO
            {
                Id = notification.Id,
                UserId = notification.UserId,
                ChannelName = channel.Name,
                Message = notification.Message,
                SendDate = notification.SendDate,
                Status = "Sent"
            };

            return dto;
        }
    }
}
