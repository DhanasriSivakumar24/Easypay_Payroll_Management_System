namespace Easypay_App.Models.DTO
{
    public class NotificationLogRequestDTO
    {
        public int UserId { get; set; }
        public int ChannelId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
