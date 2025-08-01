namespace Easypay_App.Models
{
    public class AuditTrail
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserAccount? User { get; set; } 
        public int ActionId { get; set; }
        public AuditTrailActionMaster? Action { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
        public string IPAddress { get; set; } = string.Empty;
        public AuditTrail()
        {
            
        }

        public AuditTrail(int id, int userId, UserAccount? user, int actionId, AuditTrailActionMaster? action, string entityName, int entityId, string oldValue, string newValue, DateTime timeStamp, string iPAddress)
        {
            Id = id;
            UserId = userId;
            User = user;
            ActionId = actionId;
            Action = action;
            EntityName = entityName;
            EntityId = entityId;
            OldValue = oldValue;
            NewValue = newValue;
            TimeStamp = timeStamp;
            IPAddress = iPAddress;
        }
    }
}
