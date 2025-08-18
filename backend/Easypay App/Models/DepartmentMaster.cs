namespace Easypay_App.Models
{
    public class DepartmentMaster
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string DepartmentDescription { get; set; } = string.Empty;
        public bool IsActive{ get; set; }
        public ICollection<Employee>? Employees { get; set; }
        public DepartmentMaster()
        {
            
        }

        public DepartmentMaster(int id, string departmentName, string departmentDescription, bool isActive)
        {
            Id = id;
            DepartmentName = departmentName;
            DepartmentDescription = departmentDescription;
            IsActive = isActive;
        }
    }
}
