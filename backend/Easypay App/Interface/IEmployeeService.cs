using Easypay_App.Models.DTO;

namespace Easypay_App.Interface
{
    public interface IEmployeeService
    {
        public Task<EmployeeAddResponseDTO> AddEmployee(EmployeeAddRequestDTO employeeDto);
        public Task<EmployeeAddResponseDTO> UpdateEmployee(int id, EmployeeUpdateRequestDTO employeeDto);
        public Task<EmployeeAddResponseDTO> DeleteEmployee(int id);
        public Task<IEnumerable<EmployeeAddResponseDTO>> GetAllEmployees();
        public Task<EmployeeAddResponseDTO> GetEmployeeById(int id);
        public Task<EmployeeAddResponseDTO> ChangeEmployeeUserRole(ChangeUserRoleDTO dto);
        public Task<PaginatedEmployeeResponseDTO> SearchEmployees(EmployeeSearchRequestDTO criteria);

    }
}
