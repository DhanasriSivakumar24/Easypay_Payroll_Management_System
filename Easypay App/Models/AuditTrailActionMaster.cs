namespace Easypay_App.Models
{
    public class AuditTrailActionMaster
    {
        public int Id { get; set; }
        public string ActionName { get; set; } = string.Empty;
        public ICollection<AuditTrail>? AuditTrails { get; set; }

        public AuditTrailActionMaster()
        {
            
        }

        public AuditTrailActionMaster(int id, string actionName)
        {
            Id = id;
            ActionName = actionName;
        }
    }
}
