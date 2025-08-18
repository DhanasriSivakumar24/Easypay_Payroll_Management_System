namespace Easypay_App.Models
{
    public class UserRoleMaster
    {
        public int Id { get; set; }
        public string UserRoleName { get; set; } = string.Empty;
        public string UserRoleDescription { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public ICollection<UserAccount>? UserAccounts { get; set; }
        public ICollection<Employee>? Employees { get; set; }
        public UserRoleMaster()
        {
            
        }

        public UserRoleMaster(int id, string userRoleName, string userRoleDescription, bool isActive)
        {
            Id = id;
            UserRoleName = userRoleName;
            UserRoleDescription = userRoleDescription;
            IsActive = isActive;
        }
    }
}
