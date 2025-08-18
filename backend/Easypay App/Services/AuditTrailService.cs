using AutoMapper;
using Easypay_App.Exceptions;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;
using log4net;

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

        public async Task<AuditTrailResponseDTO> LogAction(AuditTrailRequestDTO dto)
        {
            try 
            {
                var user = await _userRepo.GetValueById(dto.UserName); // Get user by username
                if (user == null)
                    throw new NoItemFoundException($"User with UserName {dto.UserName} not found");

                var action = await _actionRepo.GetValueById(dto.ActionId);
                if (action == null)
                    throw new NoItemFoundException($"Action with ID {dto.ActionId} not found");

                var audit = _mapper.Map<AuditTrail>(dto);
                audit.UserId = user.Id;      
                audit.UserName = user.UserName; 
                audit.TimeStamp = DateTime.Now;

                var added = await _auditRepo.AddValue(audit);

                var result = _mapper.Map<AuditTrailResponseDTO>(added);
                result.UserName = user.UserName;
                result.ActionName = action.ActionName;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in LogAction: {ex.Message} ");
            }
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
                dto.UserName = (await _userRepo.GetValueById(log.UserId.ToString()))?.UserName ?? log.UserName ?? "Unknown";
                dto.ActionName = (await _actionRepo.GetValueById(log.ActionId))?.ActionName ?? "Unknown";
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
    }
}
