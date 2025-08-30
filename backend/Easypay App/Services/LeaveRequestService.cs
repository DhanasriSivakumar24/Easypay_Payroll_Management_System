using AutoMapper;
using Easypay_App.Exceptions;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Easypay_App.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IRepository<int, Employee> _employeeRepository;
        private readonly IRepository<int, LeaveRequest> _leaveRequestRepository;
        private readonly IRepository<int, LeaveStatusMaster> _leaveStatusRepository;
        private readonly IRepository<int, LeaveTypeMaster> _leaveTypeRepository;
        private readonly INotificationLogService _notificationService;
        private readonly IMapper _mapper;

        public LeaveRequestService(IRepository<int,Employee> employeeRepository,
            IRepository<int,LeaveRequest> leaveRequestRepository,
            IRepository<int, LeaveStatusMaster> leaveStatusRepository,
            IRepository<int, LeaveTypeMaster> leaveTypeRepository,
            INotificationLogService notificationLogService,
            IMapper mapper) 
        {
            _employeeRepository = employeeRepository;
            _leaveRequestRepository = leaveRequestRepository;
            _leaveStatusRepository = leaveStatusRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _notificationService = notificationLogService;
            _mapper = mapper;
        }
        public async Task<LeaveRequestResponseDTO> ApproveLeave(int id, int managerId, bool isApproved)
        {
            var leave = await _leaveRequestRepository.GetValueById(id);
            if (leave == null) throw new NoItemFoundException();

            leave.StatusId = isApproved ? 2 : 3; // 2 - Approved, 3 - Rejected
            leave.ApprovedBy = managerId;
            leave.ActionedAt = DateTime.Now;

            await _leaveRequestRepository.UpdateValue(id, leave);

            var response = _mapper.Map<LeaveRequestResponseDTO>(leave);
            await PopulateNames(response, leave);

            string message = isApproved
                ? $"Your leave request from {leave.StartDate:dd-MMM} to {leave.EndDate:dd-MMM} has been approved."
                : $"Your leave request from {leave.StartDate:dd-MMM} to {leave.EndDate:dd-MMM} has been rejected.";

            await _notificationService.SendNotification(new NotificationLogRequestDTO
            {
                UserId = leave.EmployeeId,
                ChannelId = 2,
                Message = message
            });

            return response;
        }

        public async Task<LeaveRequestResponseDTO> DeleteLeaveRequest(int id)
        {
            var leave = await _leaveRequestRepository.GetValueById(id);
            if (leave == null)
                throw new NoItemFoundException();
            var response = _mapper.Map< LeaveRequestResponseDTO >(leave);
            return response;
        }

        public async Task<IEnumerable<LeaveRequestResponseDTO>> GetAllLeaveRequests()
        {
            var all = await _leaveRequestRepository.GetAllValue();
            var responseList = new List<LeaveRequestResponseDTO>();

            foreach (var req in all)
            {
                var dto = _mapper.Map<LeaveRequestResponseDTO>(req);
                await PopulateNames(dto, req);
                responseList.Add(dto);
            }

            return responseList;
        }

        public async Task<LeaveRequestResponseDTO> GetLeaveRequestById(int id)
        {
            var req = await _leaveRequestRepository.GetValueById(id);
            var dto = _mapper.Map<LeaveRequestResponseDTO>(req);
            await PopulateNames(dto, req);
            return dto;
        }

        public async Task<LeaveRequestResponseDTO> RejectLeave(int id, int managerId)
        {
            var request = await _leaveRequestRepository.GetValueById(id);
            if (request == null)
                throw new NoItemFoundException();

            var statuses = await _leaveStatusRepository.GetAllValue();
            var rejectedStatus = statuses.FirstOrDefault(s => s.StatusName.ToLower() == "rejected");

            if (rejectedStatus == null)
                throw new Exception("Rejected status not found in LeaveStatusMaster");

            request.StatusId = rejectedStatus.Id;
            request.ApprovedBy = managerId;
            request.ActionedAt = DateTime.Now;

            await _leaveRequestRepository.UpdateValue(id, request);

            var response = _mapper.Map<LeaveRequestResponseDTO>(request);
            await PopulateNames(response, request);

            await _notificationService.SendNotification(new NotificationLogRequestDTO
            {
                UserId = request.EmployeeId,
                ChannelId = 1,
                Message = $"Your leave request from {request.StartDate:dd-MMM} to {request.EndDate:dd-MMM} has been rejected."
            });
            return response;
        }
        public async Task<LeaveRequestResponseDTO> SubmitLeaveRequest(LeaveRequestDTO dto)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(dto);
            leaveRequest.RequestedAt = DateTime.Now;
            leaveRequest.StatusId = 1; // Pending

            await _leaveRequestRepository.AddValue(leaveRequest);

            var response = _mapper.Map<LeaveRequestResponseDTO>(leaveRequest);
            await PopulateNames(response, leaveRequest);

            await _notificationService.SendNotification(new NotificationLogRequestDTO
            {
                UserId = leaveRequest.EmployeeId,
                ChannelId = 1,
                Message = $"Your leave request from {leaveRequest.StartDate:dd-MMM} to {leaveRequest.EndDate:dd-MMM} has been submitted and is pending approval."
            });
            return response;
        }
        private async Task PopulateNames(LeaveRequestResponseDTO dto, LeaveRequest request)
        {
            var emp = await _employeeRepository.GetValueById(request.EmployeeId);
            dto.EmployeeName = $"{emp.FirstName} {emp.LastName}";

            var type = await _leaveTypeRepository.GetValueById(request.LeaveTypeId);
            dto.LeaveTypeName = type.LeaveTypeName;

            var status = await _leaveStatusRepository.GetValueById(request.StatusId);
            dto.StatusName = status.StatusName;

            if (request.ApprovedBy.HasValue)
            {
                var approver = await _employeeRepository.GetValueById(request.ApprovedBy.Value);
                dto.ApprovedManagerName = $"{approver.FirstName} {approver.LastName}";
            }
            else
            {
                dto.ApprovedManagerName = "";
            }
        }
        public async Task<IEnumerable<LeaveRequestResponseDTO>> GetLeaveRequestsByEmployee(int employeeId)
        {
            var all = await _leaveRequestRepository.GetAllValue();
            var employeeRequests = all.Where(lr => lr.EmployeeId == employeeId).ToList();

            var responseList = new List<LeaveRequestResponseDTO>();

            foreach (var req in employeeRequests)
            {
                var dto = _mapper.Map<LeaveRequestResponseDTO>(req);
                await PopulateNames(dto, req);
                responseList.Add(dto);
            }

            return responseList;
        }
    }
}