using Easypay_App.Models.DTO;

namespace Easypay_App.Interface
{
    public interface IAuditTrailService
    {
        public Task<AuditTrailResponseDTO> LogAction(AuditTrailRequestDTO dto);
        public Task<AuditTrailResponseDTO> GetLogById(int id);
        public Task<IEnumerable<AuditTrailResponseDTO>> GetAllLogs();
        public Task<IEnumerable<AuditTrailResponseDTO>> GetLogsByUser(int userId);
        public Task<IEnumerable<AuditTrailResponseDTO>> GetLogsByAction(int actionId);
    }
}
