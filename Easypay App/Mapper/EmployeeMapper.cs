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
            CreateMap<PayrollPolicyMaster, PayrollPolicyRequestDTO>().ReverseMap();
            CreateMap<PayrollPolicyMaster, PayrollPolicyResponseDTO>().ReverseMap();
            #endregion

        }

    }
}
