namespace Easypay_App.Models.DTO
{
    public class PaginatedEmployeeResponseDTO
    {
        public List<EmployeeAddResponseDTO>? Employees { get; set; }
        public int? PageNumber { get; set; }
        public int? TotalNumberOfRecords { get; set; }
        public ErrorObjectDTO? Error { get; set; }
    }
}
