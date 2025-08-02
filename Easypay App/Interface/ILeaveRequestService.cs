using Easypay_App.Models.DTO;

namespace Easypay_App.Interface
{
    public interface ILeaveRequestService
    {
        LeaveRequestResponseDTO SubmitLeaveRequest(LeaveRequestDTO dto);
        LeaveRequestResponseDTO ApproveLeave(int id, int managerId, bool isApproved);
        LeaveRequestResponseDTO RejectLeave(int id, int managerId);
        LeaveRequestResponseDTO GetLeaveRequestById(int id);
        IEnumerable<LeaveRequestResponseDTO> GetAllLeaveRequests();
        LeaveRequestResponseDTO DeleteLeaveRequest(int id);
    }
}
