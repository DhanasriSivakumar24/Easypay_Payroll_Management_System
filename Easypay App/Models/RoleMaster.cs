namespace Easypay_App.Models
{
    public class RoleMaster
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string RoleDescription { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public ICollection<Employee>? Employees { get; set; }
        public RoleMaster()
        {
            
        }

        public RoleMaster(int id, string roleName, string roleDescription, bool isActive)
        {
            Id = id;
            RoleName = roleName;
            RoleDescription = roleDescription;
            IsActive = isActive;
        }
    }
}
