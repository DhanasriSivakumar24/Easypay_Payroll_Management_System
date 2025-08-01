namespace Easypay_App.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }=string.Empty;
        public string LastName { get; set; } =string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        public int StatusId { get; set; }
        public EmployeeStatusMaster? Status { get; set; }
        public int DepartmentId { get; set; }
        public DepartmentMaster? Department { get; set; }
        public int RoleId { get; set; }
        public RoleMaster? Role { get; set; }
        public int? ReportingManagerId { get; set; }
        public Employee? ReportingManager { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PanNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<Attendance>? Attendances { get; set; }
        public ICollection<BenefitEnrollment>? BenefitEnrollments { get; set; }
        public ICollection<Payroll>? Payrolls { get; set; }
        public UserAccount? UserAccount { get; set; }

        public Employee()
        {
            
        }

        public Employee(int id, string firstName, string lastName, DateTime dateOfBirth, string gender, DateTime joinDate, int statusId, EmployeeStatusMaster? status, int departmentId, DepartmentMaster? department, int roleId, RoleMaster? role, int reportingManagerId, Employee? reportingManager, string email, string phoneNumber, string address, string panNumber, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            JoinDate = joinDate;
            StatusId = statusId;
            Status = status;
            DepartmentId = departmentId;
            Department = department;
            RoleId = roleId;
            Role = role;
            ReportingManagerId = reportingManagerId;
            ReportingManager = reportingManager;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            PanNumber = panNumber;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
