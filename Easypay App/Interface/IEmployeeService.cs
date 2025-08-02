using Easypay_App.Models.DTO;

namespace Easypay_App.Interface
{
    public interface IEmployeeService
    {
        EmployeeAddResponseDTO AddEmployee(EmployeeAddRequestDTO employeeDto);
        EmployeeAddResponseDTO UpdateEmployee(int id, EmployeeAddRequestDTO employeeDto);
        EmployeeAddResponseDTO DeleteEmployee(int id);
        IEnumerable<EmployeeAddResponseDTO> GetAllEmployees();
        EmployeeAddResponseDTO GetEmployeeById(int id);
    }
}
