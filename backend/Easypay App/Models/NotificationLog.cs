namespace Easypay_App.Models
{
    public class NotificationLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserAccount? User { get; set; }
        public int ChannelId { get; set; }
        public NotificationChannelMaster? Channel { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime SendDate { get; set; } = DateTime.Now;
        public int StatusId { get; set; }
        public NotificationStatusMaster? Status { get; set; }
        public string StatusMessage { get; set; } = string.Empty;
        public NotificationLog()
        {
            
        }

        public NotificationLog(int id, int userId, UserAccount? user, int channelId, NotificationChannelMaster? channel, string message, DateTime sendDtate, int statusId, NotificationStatusMaster? status, string statusMessage)
        {
            Id = id;
            UserId = userId;
            User = user;
            ChannelId = channelId;
            Channel = channel;
            Message = message;
            SendDate = sendDtate;
            StatusId = statusId;
            Status = status;
            StatusMessage = statusMessage;
        }
    }
}
