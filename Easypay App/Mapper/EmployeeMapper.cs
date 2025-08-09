using AutoMapper;
using Easypay_App.Models;
using Easypay_App.Models.DTO;

namespace Easypay_App.Mapper
{
    public class EmployeeMapper : Profile
    {
        public EmployeeMapper()
        {
            #region Employee Add Mapper
            CreateMap<Employee, EmployeeAddResponseDTO>().ReverseMap();
            CreateMap<Employee, EmployeeAddRequestDTO>().ReverseMap();


            // Mapping from AddRequestDTO → Entity
            CreateMap<EmployeeAddRequestDTO, Employee>();

            // Mapping from Entity → AddResponseDTO
            CreateMap<Employee, EmployeeAddResponseDTO>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.DepartmentName : string.Empty))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.RoleName : string.Empty))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status != null ? src.Status.StatusName : string.Empty))
                .ForMember(dest => dest.ReportingManager, opt => opt.MapFrom(src => src.ReportingManagerId))
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.UserRoleName, opt => opt.MapFrom(src => src.UserRole.UserRoleName));

            #endregion

            #region Payroll Policy
            CreateMap<PayrollPolicyMaster, PayrollPolicyAddRequestDTO>().ReverseMap();
            CreateMap<PayrollPolicyMaster, PayrollPolicyAddResponseDTO>().ReverseMap();
            #endregion

            #region Benefit Enrollment
            CreateMap<BenefitEnrollmentAddRequestDTO, BenefitEnrollment>();

            CreateMap<BenefitEnrollment, BenefitEnrollmentAddResponseDTO>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FirstName + " " + src.Employee.LastName : "N/A"))
                .ForMember(dest => dest.BenefitName, opt => opt.MapFrom(src => src.Benefit != null ? src.Benefit.BenefitName : "N/A"))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status != null ? src.Status.StatusName : "N/A"));

            #endregion

            #region Leave Request
            CreateMap<LeaveRequestDTO, LeaveRequest>();
            CreateMap<LeaveRequest, LeaveRequestResponseDTO>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName))
                .ForMember(dest => dest.LeaveTypeName, opt => opt.MapFrom(src => src.LeaveType.LeaveTypeName))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.StatusName))
                .ForMember(dest => dest.ApprovedManagerName, opt => opt.MapFrom(src => src.ApprovedManager != null ? src.ApprovedManager.FirstName + " " + src.ApprovedManager.LastName : ""))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason));
            #endregion

            #region Payroll
            CreateMap<PayrollRequestDTO, Payroll>();
            CreateMap<Payroll, PayrollResponseDTO>()
                .ForMember(dest => dest.EmployeeName, opt => opt.Ignore())
                .ForMember(dest => dest.PolicyName, opt => opt.Ignore())
                .ForMember(dest => dest.StatusName, opt => opt.Ignore());
            #endregion

            #region Login
            CreateMap<RegisterRequestDTO, Employee>();
            #endregion

            #region Notification
            CreateMap<NotificationLog, NotificationLogDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.StatusName))
                .ForMember(dest => dest.ChannelName, opt => opt.MapFrom(src => src.Channel.Name));

            CreateMap<NotificationLogRequestDTO, NotificationLog>();
            #endregion

            #region AuditTrail

            CreateMap<AuditTrailRequestDTO, AuditTrail>();
            CreateMap<AuditTrail, AuditTrailResponseDTO>()
                .ForMember(dest => dest.ActionName, opt => opt.MapFrom(src => src.Action.ActionName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
            #endregion

            #region Attendance

            CreateMap<Attendance, AttendanceResponseDTO>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src =>
                    src.Employee != null ? $"{src.Employee.FirstName} {src.Employee.LastName}" : "Unknown"))
                .ForMember(dest => dest.InTime, opt => opt.MapFrom(src => src.InTime.ToString("HH:mm")))
                .ForMember(dest => dest.OutTime, opt => opt.MapFrom(src => src.OutTime.ToString("HH:mm")))
                .ForMember(dest => dest.TotalHours, opt => opt.MapFrom(src =>
                    $"{src.TotalHours.Hours}h {src.TotalHours.Minutes}m"))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src =>
                    src.Status != null ? src.Status.StatusName : "Unknown"));

            #endregion
        }

    }
}
