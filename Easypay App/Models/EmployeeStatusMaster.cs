namespace Easypay_App.Models
{
    public class EmployeeStatusMaster
    {
        public int Id { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public ICollection<Employee>? Employees { get; set; }
        public EmployeeStatusMaster()
        {
            
        }

        public EmployeeStatusMaster(int id, string statusName)
        {
            Id = id;
            StatusName = statusName;
        }
    }
}
