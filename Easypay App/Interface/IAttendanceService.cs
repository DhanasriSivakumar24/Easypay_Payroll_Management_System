using Easypay_App.Models.DTO;

namespace Easypay_App.Interface
{
    public interface IAttendanceService
    {
        public Task<AttendanceResponseDTO> MarkAttendance(AttendanceRequestDTO dto);
        public Task<IEnumerable<AttendanceResponseDTO>> GetAttendanceByEmployee(int employeeId);

    }
}
