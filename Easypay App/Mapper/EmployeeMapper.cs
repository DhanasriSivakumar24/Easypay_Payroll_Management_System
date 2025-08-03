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
                .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary)); ;
            #endregion

            #region Payroll Policy
            CreateMap<PayrollPolicyMaster, PayrollPolicyAddRequestDTO>().ReverseMap();
            CreateMap<PayrollPolicyMaster, PayrollPolicyAddResponseDTO>().ReverseMap();
            #endregion

            #region Benefit Enrollment
            CreateMap<BenefitEnrollmentAddRequestDTO, BenefitEnrollment>();
            CreateMap<BenefitEnrollment, BenefitEnrollmentAddResponseDTO>()
                .ForMember(dest => dest.EmployeeName,
                        opt => opt.MapFrom(src =>
                            src.Employee != null ? src.Employee.FirstName + " " + src.Employee.LastName : string.Empty))
                .ForMember(dest => dest.BenefitName, opt => opt.MapFrom(src => src.Benefit != null ? src.Benefit.BenefitName : string.Empty))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.StatusName));
            CreateMap<BenefitEnrollmentAddResponseDTO, BenefitEnrollment>();

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

        }

    }
}
