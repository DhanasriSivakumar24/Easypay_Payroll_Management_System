namespace Easypay_App.Models
{
    public class NotificationChannelMaster
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<NotificationLog>? NotificationLogs { get; set; }

        public NotificationChannelMaster()
        {
            
        }

        public NotificationChannelMaster(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
