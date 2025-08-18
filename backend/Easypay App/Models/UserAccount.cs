namespace Easypay_App.Models
{
    public class UserAccount
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public string UserName { get; set; } = string.Empty;
        public byte[] Password { get; set; }
        public byte[] HashKey { get; set; }
        public int UserRoleId { get; set; }
        public UserRoleMaster? Role { get; set; }
        public ICollection<AuditTrail>? AuditTrails { get; set; }
        public ICollection<NotificationLog>? NotificationLogs { get; set; }
        public bool ActiveFlag { get; set; }
        public DateTime LastLogin { get; set; }
        public UserAccount()
        {
            
        }

        public UserAccount(int id, int employeeId, Employee? employee, string userName, byte[] password, byte[] hashKey, int userRoleId, UserRoleMaster? role, ICollection<AuditTrail>? auditTrails, ICollection<NotificationLog>? notificationLogs, bool activeFlag, DateTime lastLogin)
        {
            Id = id;
            EmployeeId = employeeId;
            Employee = employee;
            UserName = userName;
            Password = password;
            HashKey = hashKey;
            UserRoleId = userRoleId;
            Role = role;
            AuditTrails = auditTrails;
            NotificationLogs = notificationLogs;
            ActiveFlag = activeFlag;
            LastLogin = lastLogin;
        }
    }
}
