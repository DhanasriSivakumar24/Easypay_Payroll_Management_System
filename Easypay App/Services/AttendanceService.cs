using AutoMapper;
using Easypay_App.Exceptions;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;

namespace Easypay_App.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IRepository<int, Attendance> _attendanceRepository;
        private readonly IRepository<int, Employee> _employeeRepository;
        private readonly IRepository<int, AttendanceStatusMaster> _statusRepository;
        private readonly IMapper _mapper;

        public AttendanceService(IRepository<int, Attendance> attendanceRepository,
                                 IRepository<int, Employee> employeeRepository,
                                 IRepository<int, AttendanceStatusMaster> statusRepository,
                                 IMapper mapper)
        {
            _attendanceRepository = attendanceRepository;
            _employeeRepository = employeeRepository;
            _statusRepository = statusRepository;
            _mapper = mapper;
        }

        public async Task<AttendanceResponseDTO> MarkAttendance(AttendanceRequestDTO dto)
        {
            var employee = await _employeeRepository.GetValueById(dto.EmployeeId);
            if (employee == null)
                throw new NoItemFoundException();

            var totalHours = dto.OutTime - dto.InTime;

            var attendance = new Attendance
            {
                EmployeeId = dto.EmployeeId,
                WorkDate = dto.WorkDate.Date,
                InTime = dto.InTime,
                OutTime = dto.OutTime,
                TotalHours = totalHours,
                StatusId = dto.StatusId,
                CreatedAt = DateTime.Now
            };

            await _attendanceRepository.AddValue(attendance);

            var status = await _statusRepository.GetValueById(attendance.StatusId);

            return new AttendanceResponseDTO
            {
                Id = attendance.Id,
                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                WorkDate = attendance.WorkDate,
                InTime = attendance.InTime.ToString("HH:mm"),
                OutTime = attendance.OutTime.ToString("HH:mm"),
                TotalHours = $"{totalHours.Hours}h {totalHours.Minutes}m",
                StatusName = status?.StatusName ?? "Unknown"
            };
        }

        public async Task<IEnumerable<AttendanceResponseDTO>> GetAttendanceByEmployee(int employeeId)
        {
            var records = await _attendanceRepository.GetAllValue();
            var employee = await _employeeRepository.GetValueById(employeeId);
            if (employee == null)
                throw new NoItemFoundException();

            return records
                .Where(a => a.EmployeeId == employeeId)
                .Select(a => new AttendanceResponseDTO
                {
                    Id = a.Id,
                    EmployeeName = $"{employee.FirstName} {employee.LastName}",
                    WorkDate = a.WorkDate,
                    InTime = a.InTime.ToString("HH:mm"),
                    OutTime = a.OutTime.ToString("HH:mm"),
                    TotalHours = $"{a.TotalHours.Hours}h {a.TotalHours.Minutes}m",
                    StatusName = a.Status?.StatusName ?? "Unknown"
                })
                .ToList();
        }
    }
}
