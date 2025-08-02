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
            // Mapping from AddRequestDTO → Entity
            CreateMap<EmployeeAddRequestDTO, Employee>();

            // Mapping from Entity → AddResponseDTO
            CreateMap<Employee, EmployeeAddResponseDTO>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.DepartmentName : string.Empty))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.RoleName : string.Empty))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status != null ? src.Status.StatusName : string.Empty))
                .ForMember(dest => dest.ReportingManager, opt => opt.MapFrom(src => src.ReportingManagerId));
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

        }

    }
}
