using Easypay_App.Models.DTO;

namespace Easypay_App.Interface
{
    public interface INotificationLogService
    {
        public Task<NotificationLogDTO> SendNotification(NotificationLogRequestDTO request);
        public Task<IEnumerable<NotificationLogDTO>> GetNotificationsByUser(int userId);
    }
}
