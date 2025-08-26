using Easypay_App.Models.DTO;

namespace Easypay_App.Interface
{
    public interface ILeaveRequestService
    {
        public Task<LeaveRequestResponseDTO> SubmitLeaveRequest(LeaveRequestDTO dto);
        public Task<LeaveRequestResponseDTO> ApproveLeave(int id, int managerId, bool isApproved);
        public Task<LeaveRequestResponseDTO> RejectLeave(int id, int managerId);
        public Task<LeaveRequestResponseDTO> GetLeaveRequestById(int id);
        public Task<IEnumerable<LeaveRequestResponseDTO>> GetAllLeaveRequests();
        public Task<LeaveRequestResponseDTO> DeleteLeaveRequest(int id);
        public Task<IEnumerable<LeaveRequestResponseDTO>> GetLeaveRequestsByEmployee(int employeeId);

    }
}
