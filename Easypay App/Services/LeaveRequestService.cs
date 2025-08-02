using AutoMapper;
using Easypay_App.Exceptions;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;

namespace Easypay_App.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IRepository<int, Employee> _employeeRepository;
        private readonly IRepository<int, LeaveRequest> _leaveRequestRepository;
        private readonly IRepository<int, LeaveStatusMaster> _leaveStatusRepository;
        private readonly IRepository<int, LeaveTypeMaster> _leaveTypeRepository;
        private readonly IMapper _mapper;

        public LeaveRequestService(IRepository<int,Employee> employeeRepository,
            IRepository<int,LeaveRequest> leaveRequestRepository,
            IRepository<int, LeaveStatusMaster> leaveStatusRepository,
            IRepository<int, LeaveTypeMaster> leaveTypeRepository,
            IMapper mapper) 
        {
            _employeeRepository = employeeRepository;
            _leaveRequestRepository = leaveRequestRepository;
            _leaveStatusRepository = leaveStatusRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _mapper = mapper;
        }
        public LeaveRequestResponseDTO ApproveLeave(int id, int managerId, bool isApproved)
        {
            var leave = _leaveRequestRepository.GetValueById(id);
            if (leave == null) throw new NoItemFoundException();

            leave.StatusId = isApproved ? 2 : 3; // 2 - Approved, 3 - Rejected
            leave.ApprovedBy = managerId;
            leave.ActionedAt = DateTime.Now;

            _leaveRequestRepository.UpdateValue(id, leave);

            var response = _mapper.Map<LeaveRequestResponseDTO>(leave);
            PopulateNames(response, leave);
            return response;
        }
        public LeaveRequestResponseDTO DeleteLeaveRequest(int id)
        {
            var leave = _leaveRequestRepository.GetValueById(id);
            if (leave == null)
                throw new NoItemFoundException();
            var response = _mapper.Map< LeaveRequestResponseDTO >(leave);
            return response;
        }

        public IEnumerable<LeaveRequestResponseDTO> GetAllLeaveRequests()
        {
            var all = _leaveRequestRepository.GetAllValue();
            return all.Select(req =>
            {
                var dto = _mapper.Map<LeaveRequestResponseDTO>(req);
                PopulateNames(dto, req);
                return dto;
            });
        }

        public LeaveRequestResponseDTO GetLeaveRequestById(int id)
        {
            var req = _leaveRequestRepository.GetValueById(id);
            var dto = _mapper.Map<LeaveRequestResponseDTO>(req);
            PopulateNames(dto, req);
            return dto;
        }

        public LeaveRequestResponseDTO RejectLeave(int id, int managerId)
        {
            var request = _leaveRequestRepository.GetValueById(id);
            if (request == null)
                throw new NoItemFoundException();

            var rejectedStatus = _leaveStatusRepository.GetAllValue()
                                 .FirstOrDefault(s => s.StatusName.ToLower() == "rejected");

            if (rejectedStatus == null)
                throw new Exception("Rejected status not found in LeaveStatusMaster");

            request.StatusId = rejectedStatus.Id;
            request.ApprovedBy = managerId;
            request.ActionedAt = DateTime.Now;

            _leaveRequestRepository.UpdateValue(id, request);

            var response = _mapper.Map<LeaveRequestResponseDTO>(request);
            PopulateNames(response, request);
            return response;
        }
        public LeaveRequestResponseDTO SubmitLeaveRequest(LeaveRequestDTO dto)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(dto);
            leaveRequest.RequestedAt = DateTime.Now;
            leaveRequest.StatusId = 1; // Pending

            _leaveRequestRepository.AddValue(leaveRequest);

            var response = _mapper.Map<LeaveRequestResponseDTO>(leaveRequest);
            PopulateNames(response, leaveRequest);
            return response;
        }
        private void PopulateNames(LeaveRequestResponseDTO dto, LeaveRequest request)
        {
            var emp = _employeeRepository.GetValueById(request.EmployeeId);
            var type = _leaveTypeRepository.GetValueById(request.LeaveTypeId);
            var status = _leaveStatusRepository.GetValueById(request.StatusId);

            Employee? approver = null;
            if (request.ApprovedBy.HasValue)
            {
                approver = _employeeRepository.GetValueById(request.ApprovedBy.Value);
            }

            dto.EmployeeName = $"{emp.FirstName} {emp.LastName}";
            dto.LeaveTypeName = type.LeaveTypeName;
            dto.StatusName = status.StatusName;
            dto.ApprovedManagerName = approver != null ? $"{approver.FirstName} {approver.LastName}" : "";
        }
    }
}