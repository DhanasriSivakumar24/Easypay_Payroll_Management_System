using Easypay_App.Models.DTO;

namespace Easypay_App.Interface
{
    public interface INotificationLogService
    {
        NotificationLogDTO SendNotification(NotificationLogRequestDTO request);
        IEnumerable<NotificationLogDTO> GetNotificationsByUser(int userId);
    }
}
