namespace Easypay_App.Models
{
    public class NotificationStatusMaster
    {
        public int Id { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public ICollection<NotificationLog>? NotificationLogs { get; set; }

        public NotificationStatusMaster()
        {
            
        }

        public NotificationStatusMaster(int id, string statusName)
        {
            Id = id;
            StatusName = statusName;
        }
    }
}
