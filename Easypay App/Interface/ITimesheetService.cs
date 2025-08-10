using Easypay_App.Models.DTO;
using System.Threading.Tasks;

namespace Easypay_App.Interface
{
    public interface ITimesheetService
    {
        public Task<TimesheetResponseDTO> AddTimesheet(TimesheetRequestDTO request);
        public Task<IEnumerable<TimesheetResponseDTO>> GetTimesheetsByEmployee(int employeeId);
        public Task<IEnumerable<TimesheetResponseDTO>> GetTimesheetsByDateRange(DateTime startDate, DateTime endDate);
        public Task<bool> ApproveTimesheet(int timesheetId);
        public Task<bool> RejectTimesheet(int timesheetId);
    }
}
