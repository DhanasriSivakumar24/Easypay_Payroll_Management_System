using AutoMapper;
using Easypay_App.Exceptions;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;
using log4net;
using Newtonsoft.Json;

namespace Easypay_App.Services
{
    public class AuditTrailService : IAuditTrailService
    {
        private readonly IRepository<int, AuditTrail> _auditRepo;
        private IRepository<string, UserAccount> _userRepo;
        private readonly IRepository<int, AuditTrailActionMaster> _actionRepo;
        private readonly IMapper _mapper;

        public AuditTrailService(
            IRepository<int, AuditTrail> auditRepo,
            IRepository<string, UserAccount> userRepo,
            IRepository<int, AuditTrailActionMaster> actionRepo,
            IMapper mapper)
        {
            _auditRepo = auditRepo;
            _userRepo = userRepo;
            _actionRepo = actionRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AuditTrailResponseDTO>> GetAllLogs()
        {
            var logs = await _auditRepo.GetAllValue();
            return await AuditResponseDTO(logs);
        }

        public async Task<IEnumerable<AuditTrailResponseDTO>> GetLogsByUser(string userName)
        {
            var logs = (await _auditRepo.GetAllValue()).Where(x => x.UserName == userName);
            return await AuditResponseDTO(logs);
        }

        public async Task<IEnumerable<AuditTrailResponseDTO>> GetLogsByAction(int actionId)
        {
            var logs = (await _auditRepo.GetAllValue()).Where(x => x.ActionId == actionId);
            return await AuditResponseDTO(logs);
        }

        private async Task<List<AuditTrailResponseDTO>> AuditResponseDTO(IEnumerable<AuditTrail> logs)
        {
            var result = new List<AuditTrailResponseDTO>();

            foreach (var log in logs)
            {
                var dto = _mapper.Map<AuditTrailResponseDTO>(log);

                try
                {
                    var user = await _userRepo.GetValueById(log.UserId.ToString());
                    dto.UserName = user?.UserName ?? log.UserName ?? "Unknown";
                }
                catch (NoItemFoundException)
                {
                    dto.UserName = log.UserName ?? "Unknown";
                }
                try
                {
                    var action = await _actionRepo.GetValueById(log.ActionId);
                    dto.ActionName = action?.ActionName ?? "Unknown";
                }
                catch (NoItemFoundException)
                {
                    dto.ActionName = "Unknown";
                }

                result.Add(dto);
            }

            return result;
        }

        public async Task<AuditTrailResponseDTO> GetLogById(int id)
        {
            var audit = await _auditRepo.GetValueById(id);
            if (audit == null)
                throw new NoItemFoundException("Audit log not found");

            var result = _mapper.Map<AuditTrailResponseDTO>(audit);
            return result;
        }

        public async Task LogAction(string userName, int actionId, string entityName, int entityId, object? oldValue = null, object? newValue = null, string? ipAddress = null)
        {
            var user = await _userRepo.GetValueById(userName);
            if (user == null)
                throw new NoItemFoundException($"User {userName} not found");

            var action = await _actionRepo.GetValueById(actionId);
            if (action == null)
                throw new NoItemFoundException($"Action with ID {actionId} not found");

            var audit = new AuditTrail
            {
                UserId = user.Id,
                UserName = user.UserName,
                ActionId = actionId,
                EntityName = entityName,
                EntityId = entityId,
                OldValue = oldValue != null ? JsonConvert.SerializeObject(oldValue) : string.Empty,
                NewValue = newValue != null ? JsonConvert.SerializeObject(newValue) : string.Empty,
                TimeStamp = DateTime.UtcNow,
                IPAddress = ipAddress ?? string.Empty
            };

            await _auditRepo.AddValue(audit);
        }
    }
}