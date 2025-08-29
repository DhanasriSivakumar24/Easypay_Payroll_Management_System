using Easypay_App.Models.DTO;

namespace Easypay_App.Interface
{
    public interface IAuditTrailService
    {
        public Task LogAction(string userName,
            int actionId,
            string entityName,
            int entityId,
            object? oldValue = null,
            object? newValue = null,
            string? ipAddress = null);

        public Task<AuditTrailResponseDTO> GetLogById(int id);
        public Task<IEnumerable<AuditTrailResponseDTO>> GetAllLogs();
        public Task<IEnumerable<AuditTrailResponseDTO>> GetLogsByUser(string userName);
        public Task<IEnumerable<AuditTrailResponseDTO>> GetLogsByAction(int actionId);
    }
}
