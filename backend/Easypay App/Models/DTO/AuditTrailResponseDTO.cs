namespace Easypay_App.Models.DTO
{
    public class AuditTrailResponseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ActionId { get; set; }
        public string? UserName { get; set; }
        public string? ActionName { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
        public string IPAddress { get; set; } = string.Empty;
    }
}
