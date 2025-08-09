namespace Easypay_App.Models.DTO
{
    public class AuditTrailRequestDTO
    {
        public int UserId { get; set; }
        public int ActionId { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;
        public string IPAddress { get; set; } = string.Empty;
    }
}
