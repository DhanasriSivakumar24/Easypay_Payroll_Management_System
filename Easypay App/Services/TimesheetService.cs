using AutoMapper;
using Easypay_App.Exceptions;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;

namespace Easypay_App.Services
{
    public class TimesheetService : ITimesheetService
    {
        private readonly IRepository<int, Timesheet> _timesheetRepository;
        private readonly IRepository<int, Employee> _employeeRepository;
        private readonly IRepository<int, TimesheetStatusMaster> _statusRepository;
        private readonly IMapper _mapper;

        public TimesheetService(
            IRepository<int, Timesheet> timesheetRepository,
            IRepository<int, Employee> employeeRepository,
            IRepository<int, TimesheetStatusMaster> statusRepository,
            IMapper mapper)
        {
            _timesheetRepository = timesheetRepository;
            _employeeRepository = employeeRepository;
            _statusRepository = statusRepository; 
            _mapper = mapper;
        }
        public async Task<TimesheetResponseDTO> AddTimesheet(TimesheetRequestDTO request)
        {
            // validate employee exists
            var employee = await _employeeRepository.GetValueById(request.EmployeeId);
            if (employee == null)
                throw new NoItemFoundException($"Employee with id {request.EmployeeId} not found.");
            // find default status (Pending) - prefer by name; fallback to id 1 if not found
            var allStatuses = await _statusRepository.GetAllValue();
            var pending = allStatuses.FirstOrDefault(s => s.StatusName.ToLower() == "pending");

            if (pending == null)
            {
                pending = await _statusRepository.GetValueById(1);
            }

            if (pending == null)
                throw new NoItemFoundException("Timesheet default status (Pending) not found in TimesheetStatusMaster.");

            var timesheet = _mapper.Map<Timesheet>(request);
            timesheet.StatusId = pending.Id;
            timesheet.CreatedAt = DateTime.Now;

            await _timesheetRepository.AddValue(timesheet);

            // Map response and populate names
            var response = _mapper.Map<TimesheetResponseDTO>(timesheet);
            await PopulateNames(response, timesheet);

            return response;
        }

        // Approve timesheet
        public async Task<bool> ApproveTimesheet(int timesheetId)
        {
            var timesheet = await _timesheetRepository.GetValueById(timesheetId);
            if (timesheet == null)
                throw new NoItemFoundException($"Timesheet with id {timesheetId} not found.");

            // find approved status
            var allStatuses = await _statusRepository.GetAllValue();
            var approved = allStatuses.FirstOrDefault(s => s.StatusName.ToLower() == "approved");

            if (approved == null)
            {
                approved = await _statusRepository.GetValueById(2);
            }

            if (approved == null)
                throw new NoItemFoundException("Timesheet status 'Approved' not found in TimesheetStatusMaster.");

            timesheet.StatusId = approved.Id;
            await _timesheetRepository.UpdateValue(timesheet.Id, timesheet);
            return true;
        }

        // Reject timesheet
        public async Task<bool> RejectTimesheet(int timesheetId)
        {
            var timesheet = await _timesheetRepository.GetValueById(timesheetId);
            if (timesheet == null)
                throw new NoItemFoundException($"Timesheet with id {timesheetId} not found.");

            var allStatuses = await _statusRepository.GetAllValue();

            var rejected = allStatuses.FirstOrDefault(s =>
                s.StatusName.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
                ?? await _statusRepository.GetValueById(3);

            if (rejected == null)
                throw new NoItemFoundException("Timesheet status 'Rejected' not found in TimesheetStatusMaster.");

            timesheet.StatusId = rejected.Id;
            await _timesheetRepository.UpdateValue(timesheet.Id, timesheet);
            return true;
        }

        // Get timesheets in date range (inclusive)
        public async Task<IEnumerable<TimesheetResponseDTO>> GetTimesheetsByDateRange(DateTime startDate, DateTime endDate)
        {
            var all = await _timesheetRepository.GetAllValue();
            var filtered = all.Where(t => t.WorkDate.Date >= startDate.Date && t.WorkDate.Date <= endDate.Date).ToList();

            if (!filtered.Any())
                throw new NoItemFoundException($"No timesheets found between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd}.");

            var result = new List<TimesheetResponseDTO>();
            foreach (var val in filtered)
            {
                var dto = _mapper.Map<TimesheetResponseDTO>(val);
                await PopulateNames(dto, val);
                result.Add(dto);
            }

            return result;
        }

        // Get timesheets by employee
        public async Task<IEnumerable<TimesheetResponseDTO>> GetTimesheetsByEmployee(int employeeId)
        {
            var all = await _timesheetRepository.GetAllValue();
            var filtered = all.Where(t => t.EmployeeId == employeeId).OrderByDescending(t => t.WorkDate).ToList();

            if (!filtered.Any())
                throw new NoItemFoundException($"No timesheets found for employee id {employeeId}.");

            var result = new List<TimesheetResponseDTO>();
            foreach (var ts in filtered)
            {
                var dto = _mapper.Map<TimesheetResponseDTO>(ts);
                await PopulateNames(dto, ts);
                result.Add(dto);
            }

            return result;
        }

        // Helper to populate EmployeeName and StatusName for a mapped DTO
        private async Task PopulateNames(TimesheetResponseDTO dto, Timesheet timesheet)
        {
            // Employee
            var emp = await _employeeRepository.GetValueById(timesheet.EmployeeId);
            dto.EmployeeName = emp != null ? $"{emp.FirstName} {emp.LastName}" : "Unknown";

            // Status
            var status = await _statusRepository.GetValueById(timesheet.StatusId);
            dto.StatusName = status?.StatusName ?? "Unknown";
        }
    }
}
