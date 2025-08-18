namespace Easypay_App.Models.DTO
{
    public class SearchRange<T>
    {
        public T MinValue { get; set; }
        public T MaxValue { get; set; }
    }
    public class EmployeeSearchRequestDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public List<int>? Departments { get; set; }
        public List<int>? RoleId { get; set; }
        public int? StatusId { get; set; }
        public List<int>? UserRoleId { get; set; }
        public List<int>? ReportingManagerId { get; set; }
        public SearchRange<decimal>? SalaryRange { get; set; }
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 5;

        //1- By ID asc, -1 - By ID desc, 2- By name asc, -2 - by name desc, 3- By department id asc, -3 by department id desc
        public int Sort { get; set; }
    }
}
