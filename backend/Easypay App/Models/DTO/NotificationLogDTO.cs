namespace Easypay_App.Models.DTO
{
    public class NotificationLogDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ChannelName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime SendDate { get; set; }
        public string Status { get; set; }
    }
}

