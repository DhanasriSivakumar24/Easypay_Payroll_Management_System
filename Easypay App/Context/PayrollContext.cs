using Easypay_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Easypay_App.Context
{
    public class PayrollContext: DbContext
    {
        public PayrollContext(DbContextOptions options) : base(options)
        {

        }
        #region DbSets

        // Master Tables
        public DbSet<AttendanceStatusMaster> AttendanceStatusMasters { get; set; }
        public DbSet<AuditTrailActionMaster> AuditTrailActionMasters { get; set; }
        public DbSet<BenefitMaster> BenefitMasters { get; set; }
        public DbSet<BenefitStatusMaster> BenefitStatusMasters { get; set; }
        public DbSet<DepartmentMaster> DepartmentMasters { get; set; }
        public DbSet<EmployeeStatusMaster> EmployeeStatusMasters { get; set; }
        public DbSet<LeaveStatusMaster> LeaveStatusMasters { get; set; }
        public DbSet<LeaveTypeMaster> LeaveTypeMasters { get; set; }
        public DbSet<NotificationChannelMaster> NotificationChannelMasters { get; set; }
        public DbSet<NotificationStatusMaster> NotificationStatusMasters { get; set; }
        public DbSet<PayrollPolicyMaster> PayrollPolicyMasters { get; set; }
        public DbSet<PayrollStatusMaster> PayrollStatusMasters { get; set; }
        public DbSet<RoleMaster> RoleMasters { get; set; }
        public DbSet<TaxMaster> TaxMasters { get; set; }
        public DbSet<UserRoleMaster> UserRoleMasters { get; set; }

        // Transactional Tables
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }
        public DbSet<BenefitEnrollment> BenefitEnrollments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<NotificationLog> NotificationLogs { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<PayrollDetail> PayrollDetails { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Timesheet> Timesheets { get; set; }
        public DbSet<TimesheetStatusMaster> TimesheetStatusMasters { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region AttendanceStatusMaster
            modelBuilder.Entity<AttendanceStatusMaster>()
                .HasKey(e => e.Id)
                .HasName("PK_Attendance_Status_Master");

            modelBuilder.Entity<AttendanceStatusMaster>()
                .HasData(
                new AttendanceStatusMaster() { Id = 1, StatusName = "Present" },
                new AttendanceStatusMaster() { Id = 2, StatusName = "Absent" },
                new AttendanceStatusMaster() { Id = 3, StatusName = "Leave" },
                new AttendanceStatusMaster() { Id = 4, StatusName = "Half-Day" },
                new AttendanceStatusMaster() { Id = 5, StatusName = "Work From Home" },
                new AttendanceStatusMaster() { Id = 6, StatusName = "Holiday" }
            );
            #endregion

            #region AuditTrailActionMaster
            modelBuilder.Entity<AuditTrailActionMaster>()
                .HasKey(e => e.Id)
                .HasName("PK_Audit_Trail_Action_Master");

            modelBuilder.Entity<AuditTrailActionMaster>()
                .HasData(
                    new AuditTrailActionMaster() { Id = 1, ActionName = "Create" },
                    new AuditTrailActionMaster() { Id = 2, ActionName = "Update" },
                    new AuditTrailActionMaster() { Id = 3, ActionName = "Delete" },
                    new AuditTrailActionMaster() { Id = 4, ActionName = "Login" },
                    new AuditTrailActionMaster() { Id = 5, ActionName = "Logout" },
                    new AuditTrailActionMaster() { Id = 6, ActionName = "Apply Leave" },
                    new AuditTrailActionMaster() { Id = 7, ActionName = "Approve Leave" },
                    new AuditTrailActionMaster() { Id = 8, ActionName = "Reject Leave" },
                    new AuditTrailActionMaster() { Id = 9, ActionName = "Run Payroll" },
                    new AuditTrailActionMaster() { Id = 10, ActionName = "Mark Attendance" }
                );
            #endregion

            #region DepartmentMaster
            modelBuilder.Entity<DepartmentMaster>()
                .HasKey(e => e.Id)
                .HasName("PK_Department_Master");

            modelBuilder.Entity<DepartmentMaster>()
                .HasData(
                    new DepartmentMaster() { Id = 1, DepartmentName = "Human Resources" },
                    new DepartmentMaster() { Id = 2, DepartmentName = "Finance" },
                    new DepartmentMaster() { Id = 3, DepartmentName = "Engineering" },
                    new DepartmentMaster() { Id = 4, DepartmentName = "Marketing" },
                    new DepartmentMaster() { Id = 5, DepartmentName = "Sales" }
                );
            #endregion

            #region BenefitStatusMaster
            modelBuilder.Entity<BenefitStatusMaster>()
                .HasKey(e => e.Id)
                .HasName("PK_Benefit_Status_Master");

            modelBuilder.Entity<BenefitStatusMaster>()
                .HasData(
                    new BenefitStatusMaster() { Id = 1, StatusName = "Pending" },
                    new BenefitStatusMaster() { Id = 2, StatusName = "Approved" },
                    new BenefitStatusMaster() { Id = 3, StatusName = "Rejected" },
                    new BenefitStatusMaster() { Id = 4, StatusName = "Cancelled" }
                );
            #endregion

            #region EmployeeStatusMaster
            modelBuilder.Entity<EmployeeStatusMaster>()
                .HasKey(e => e.Id)
                .HasName("PK_Employee_Status_Master");

            modelBuilder.Entity<EmployeeStatusMaster>()
                .HasData(
                    new EmployeeStatusMaster() { Id = 1, StatusName = "Active" },
                    new EmployeeStatusMaster() { Id = 2, StatusName = "On Leave" },
                    new EmployeeStatusMaster() { Id = 3, StatusName = "Terminated" },
                    new EmployeeStatusMaster() { Id = 4, StatusName = "Resigned" }
                );
            #endregion

            #region LeaveStatusMaster
            modelBuilder.Entity<LeaveStatusMaster>()
                .HasKey(e => e.Id)
                .HasName("PK_Leave_Status_Master");

            modelBuilder.Entity<LeaveStatusMaster>()
                .HasData(
                    new LeaveStatusMaster() { Id = 1, StatusName = "Pending" },
                    new LeaveStatusMaster() { Id = 2, StatusName = "Approved" },
                    new LeaveStatusMaster() { Id = 3, StatusName = "Rejected" },
                    new LeaveStatusMaster() { Id = 4, StatusName = "Cancelled" }
                );
            #endregion

            #region LeaveTypeMaster
            modelBuilder.Entity<LeaveTypeMaster>()
                .HasKey(e => e.Id)
                .HasName("PK_Leave_Type_Master");

            modelBuilder.Entity<LeaveTypeMaster>()
                .HasData(
                    new LeaveTypeMaster() { Id = 1, LeaveTypeName = "Sick Leave" },
                    new LeaveTypeMaster() { Id = 2, LeaveTypeName = "Casual Leave" },
                    new LeaveTypeMaster() { Id = 3, LeaveTypeName = "Earned Leave" },
                    new LeaveTypeMaster() { Id = 4, LeaveTypeName = "Maternity Leave" },
                    new LeaveTypeMaster() { Id = 5, LeaveTypeName = "Paternity Leave" },
                    new LeaveTypeMaster() { Id = 6, LeaveTypeName = "Compensatory Off" }
                );
            #endregion

            #region BenefitMaster
            modelBuilder.Entity<BenefitMaster>()
                .HasKey(e => e.Id)
                .HasName("PK_Benefit_Master");

            modelBuilder.Entity<BenefitMaster>()
                .HasData(
                    new BenefitMaster() { Id = 1, BenefitName = "House Rent Allowance (HRA)" },
                    new BenefitMaster() { Id = 2, BenefitName = "Transport Allowance" },
                    new BenefitMaster() { Id = 3, BenefitName = "Performance Bonus" },
                    new BenefitMaster() { Id = 4, BenefitName = "Medical Reimbursement" },
                    new BenefitMaster() { Id = 5, BenefitName = "Provident Fund (PF)" }
                );
            #endregion

            #region NotificationChannelMaster
            modelBuilder.Entity<NotificationChannelMaster>()
                .HasKey(e => e.Id)
                .HasName("PK_Notification_Channel_Master");

            modelBuilder.Entity<NotificationChannelMaster>()
                .HasData(
                    new NotificationChannelMaster() { Id = 1, Name = "Email" },
                    new NotificationChannelMaster() { Id = 2, Name = "SMS" },
                    new NotificationChannelMaster() { Id = 3, Name = "In-App" }
                );
            #endregion

            #region NotificationStatusMaster
            modelBuilder.Entity<NotificationStatusMaster>()
                .HasKey(e => e.Id)
                .HasName("PK_Notification_Status_Master");

            modelBuilder.Entity<NotificationStatusMaster>()
                .HasData(
                    new NotificationStatusMaster() { Id = 1, StatusName = "Pending" },
                    new NotificationStatusMaster() { Id = 2, StatusName = "Sent" },
                    new NotificationStatusMaster() { Id = 3, StatusName = "Failed" }
                );
            #endregion

            #region PayrollStatusMaster
            modelBuilder.Entity<PayrollStatusMaster>()
                .HasKey(e => e.Id)
                .HasName("PK_Payroll_Status_Master");

            modelBuilder.Entity<PayrollStatusMaster>()
                .HasData(
                    new PayrollStatusMaster() { Id = 1, StatusName = "Pending" },
                    new PayrollStatusMaster() { Id = 2, StatusName = "Processed" },
                    new PayrollStatusMaster() { Id = 3, StatusName = "Approved" },
                    new PayrollStatusMaster() { Id = 4, StatusName = "Rejected" }
                );
            #endregion

            #region RoleMaster
            modelBuilder.Entity<RoleMaster>()
                .HasKey(e => e.Id)
                .HasName("PK_Role_Master");

            modelBuilder.Entity<RoleMaster>()
                .HasData(
                    new RoleMaster() { Id = 1, RoleName = "Admin" },
                    new RoleMaster() { Id = 2, RoleName = "HR Manager" },
                    new RoleMaster() { Id = 3, RoleName = "Employee" }
                );
            #endregion

            #region PayrollPolicyMaster
            modelBuilder.Entity<PayrollPolicyMaster>()
                .HasKey(p => p.Id)
                .HasName("PK_Payroll_Policy_Master");
            modelBuilder.Entity<PayrollPolicyMaster>(entity =>
            {
                entity.Property(p => p.BasicPercent).HasPrecision(18, 4);
                entity.Property(p => p.EmployeePercent).HasPrecision(18, 4);
                entity.Property(p => p.EmployerPercent).HasPrecision(18, 4);
                entity.Property(p => p.GratuityPercent).HasPrecision(18, 4);
                entity.Property(p => p.HRAPercent).HasPrecision(18, 4);
                entity.Property(p => p.MedicalPercent).HasPrecision(18, 4);
                entity.Property(p => p.SpecialPercent).HasPrecision(18, 4);
                entity.Property(p => p.TravelPercent).HasPrecision(18, 4);
            });

            modelBuilder.Entity<PayrollPolicyMaster>()
                .HasData(
                    new PayrollPolicyMaster
                    {
                        Id = 1,
                        PolicyName = "Default Policy FY25",
                        BasicPercent = 40,
                        HRAPercent = 20,
                        SpecialPercent = 10,
                        TravelPercent = 5,
                        MedicalPercent = 5,
                        EmployeePercent = 12,   // Employee PF contribution
                        EmployerPercent = 12,   // Employer PF contribution
                        GratuityPercent = 4.81m,
                        TaxRegime = "New",
                        EffectiveFrom = new DateTime(2025, 4, 1),
                        EffectiveTo = new DateTime(2026, 3, 31),
                        IsActive = true
                    }
                );
            #endregion

            #region TaxMaster
            modelBuilder.Entity<TaxMaster>()
                .HasKey(t => t.Id)
                .HasName("PK_Tax_Master");
            modelBuilder.Entity<TaxMaster>(entity =>
            {
                entity.Property(t => t.TaxRate).HasPrecision(18, 4);
            });

            modelBuilder.Entity<TaxMaster>().HasData(
                new TaxMaster { Id = 1, MinIncome = 0, MaxIncome = 250000, TaxRate = 0.00m, TaxAmount = 0 },
                new TaxMaster { Id = 2, MinIncome = 250001, MaxIncome = 500000, TaxRate = 0.05m, TaxAmount = 12500 },
                new TaxMaster { Id = 3, MinIncome = 500001, MaxIncome = 1000000, TaxRate = 0.20m, TaxAmount = 100000 },
                new TaxMaster { Id = 4, MinIncome = 1000001, MaxIncome = 9999999, TaxRate = 0.30m, TaxAmount = 300000 }
            );
            #endregion

            #region UserRoleMaster
            modelBuilder.Entity<UserRoleMaster>()
                .HasKey(r => r.Id)
                .HasName("PK_User_Role_Master");

            modelBuilder.Entity<UserRoleMaster>().HasData(
                new UserRoleMaster { Id = 1, UserRoleName = "Admin", UserRoleDescription = "System Administrator", IsActive = true },
                new UserRoleMaster { Id = 2, UserRoleName = "HR Manager", UserRoleDescription = "Manages Payroll and Leaves", IsActive = true },
                new UserRoleMaster { Id = 3, UserRoleName = "Employee", UserRoleDescription = "General Employee User", IsActive = true }
            );
            #endregion

            #region Attendance
            modelBuilder.Entity<Attendance>().HasKey(a => a.Id).HasName("PK_Attendance");
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.Attendances)
                .HasForeignKey(a => a.EmployeeId)
                .HasConstraintName("FK_Employee_Id")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Status)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.StatusId)
                .HasConstraintName("FK_Attendance_AttendanceStatus")
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region AuditTrail
            modelBuilder.Entity<AuditTrail>().HasKey(a => a.Id).HasName("PK_AuditTrail");
            modelBuilder.Entity<AuditTrail>()
                .HasOne(at => at.User)
                .WithMany(u => u.AuditTrails)
                .HasForeignKey(at => at.UserId)
                .HasConstraintName("FK_AuditTrail_UserAccount");

            modelBuilder.Entity<AuditTrail>()
                .HasOne(at => at.Action)
                .WithMany(a => a.AuditTrails)
                .HasForeignKey(at => at.ActionId)
                .HasConstraintName("FK_AuditTrail_ActionMaster");
            #endregion

            #region BenefitEnrollment
            modelBuilder.Entity<BenefitEnrollment>()
                .HasKey(be => be.Id)
                .HasName("PK_BenefitEnrollment");

            modelBuilder.Entity<BenefitEnrollment>(entity =>
            {
                entity.Property(b => b.EmployeeContribution).HasPrecision(18, 4);
                entity.Property(b => b.EmployerContribution).HasPrecision(18, 4);
            });

            modelBuilder.Entity<BenefitEnrollment>()
                .HasOne(be => be.Employee)
                .WithMany(e => e.BenefitEnrollments)
                .HasForeignKey(be => be.EmployeeId)
                .HasConstraintName("FK_BenefitEnrollment_Employee")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BenefitEnrollment>()
                .HasOne(be => be.Benefit)
                .WithMany(b => b.BenefitEnrollments)
                .HasForeignKey(be => be.BenefitId)
                .HasConstraintName("FK_BenefitEnrollment_BenefitMaster")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BenefitEnrollment>()
                .HasOne(be => be.Status)
                .WithMany(s => s.BenefitEnrollments)
                .HasForeignKey(be => be.StatusId)
                .HasConstraintName("FK_BenefitEnrollment_StatusMaster")
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Employee
            modelBuilder.Entity<Employee>()
                .HasKey(e => e.Id)
                .HasName("PK_Employee");

            // Department relationship
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .HasConstraintName("FK_Employee_Department")
                .OnDelete(DeleteBehavior.Restrict);

            // Role relationship
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Role)
                .WithMany(r => r.Employees)
                .HasForeignKey(e => e.RoleId)
                .HasConstraintName("FK_Employee_Role")
                .OnDelete(DeleteBehavior.Restrict);

            // Status relationship
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Status)
                .WithMany(s => s.Employees)
                .HasForeignKey(e => e.StatusId)
                .HasConstraintName("FK_Employee_EmployeeStatus")
                .OnDelete(DeleteBehavior.Restrict);

            // Reporting Manager (Self-referencing)
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.ReportingManager)
                .WithMany()
                .HasForeignKey(e => e.ReportingManagerId)
                .HasConstraintName("FK_Employee_ReportingManager")
                .OnDelete(DeleteBehavior.Restrict);

            // Attendance relationship (if needed for clarity)
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Attendances)
                .WithOne(a => a.Employee)
                .HasForeignKey(a => a.EmployeeId)
                .HasConstraintName("FK_Attendance_Employee")
                .OnDelete(DeleteBehavior.Restrict);

            // Benefit Enrollment relationship
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.BenefitEnrollments)
                .WithOne(be => be.Employee)
                .HasForeignKey(be => be.EmployeeId)
                .HasConstraintName("FK_BenefitEnrollment_Employee")
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.UserRole)
                .WithMany(e=>e.Employees)
                .HasForeignKey(e => e.UserRoleId)
                .HasConstraintName("FK_Employee_UserRole")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasColumnType("decimal(18,2)");

            #endregion

            #region LeaveRequest
            modelBuilder.Entity<LeaveRequest>()
                .HasKey(lr => lr.Id)
                .HasName("PK_LeaveRequest");

            // Employee requesting the leave
            modelBuilder.Entity<LeaveRequest>()
                .HasOne<Employee>()
                .WithMany()
                .HasForeignKey(lr => lr.EmployeeId)
                .HasConstraintName("FK_LeaveRequest_Employee")
                .OnDelete(DeleteBehavior.Restrict);

            // Leave Type relationship
            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.LeaveType)
                .WithMany(lt => lt.LeaveRequests)
                .HasForeignKey(lr => lr.LeaveTypeId)
                .HasConstraintName("FK_LeaveRequest_LeaveType")
                .OnDelete(DeleteBehavior.Restrict);

            // Leave Status relationship
            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.Status)
                .WithMany(ls => ls.LeaveRequests)
                .HasForeignKey(lr => lr.StatusId)
                .HasConstraintName("FK_LeaveRequest_Status")
                .OnDelete(DeleteBehavior.Restrict);

            // Approved By (Manager)
            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.ApprovedManager)
                .WithMany()
                .HasForeignKey(lr => lr.ApprovedBy)
                .HasConstraintName("FK_LeaveRequest_ApprovedBy")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.Employee)
                .WithMany()
                .HasForeignKey(lr => lr.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.ApprovedManager)
                .WithMany()
                .HasForeignKey(lr => lr.ApprovedBy)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region NotificationLog
            modelBuilder.Entity<NotificationLog>()
                .HasKey(nl => nl.Id)
                .HasName("PK_NotificationLog");

            // User who receives the notification
            modelBuilder.Entity<NotificationLog>()
                .HasOne(nl => nl.User)
                .WithMany(u => u.NotificationLogs)
                .HasForeignKey(nl => nl.UserId)
                .HasConstraintName("FK_NotificationLog_User")
                .OnDelete(DeleteBehavior.Restrict);

            // Notification Channel (Email, SMS, etc.)
            modelBuilder.Entity<NotificationLog>()
                .HasOne(nl => nl.Channel)
                .WithMany(c => c.NotificationLogs)
                .HasForeignKey(nl => nl.ChannelId)
                .HasConstraintName("FK_NotificationLog_Channel")
                .OnDelete(DeleteBehavior.Restrict);

            // Notification Status (Sent, Failed, Pending, etc.)
            modelBuilder.Entity<NotificationLog>()
                .HasOne(nl => nl.Status)
                .WithMany(s => s.NotificationLogs)
                .HasForeignKey(nl => nl.StatusId)
                .HasConstraintName("FK_NotificationLog_Status")
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Payroll
            modelBuilder.Entity<Payroll>()
                .HasKey(p => p.Id)
                .HasName("PK_Payroll");

            modelBuilder.Entity<Payroll>(entity =>
            {
                entity.Property(p => p.Allowances).HasPrecision(18, 4);
                entity.Property(p => p.BasicPay).HasPrecision(18, 4);
                entity.Property(p => p.Deductions).HasPrecision(18, 4);
                entity.Property(p => p.NetPay).HasPrecision(18, 4);
            });

            // Employee who is being paid
            modelBuilder.Entity<Payroll>()
                .HasOne(p => p.Employee)
                .WithMany(e => e.Payrolls)
                .HasForeignKey(p => p.EmployeeId)
                .HasConstraintName("FK_Payroll_Employee")
                .OnDelete(DeleteBehavior.Restrict);

            // Payroll policy used for computation
            modelBuilder.Entity<Payroll>()
                .HasOne(p => p.Policy)
                .WithMany(pp => pp.Payrolls)
                .HasForeignKey(p => p.PolicyId)
                .HasConstraintName("FK_Payroll_Policy")
                .OnDelete(DeleteBehavior.Restrict);

            // Status of the payroll (Pending, Approved, Paid, etc.)
            modelBuilder.Entity<Payroll>()
                .HasOne(p => p.Status)
                .WithMany(s => s.Payrolls)
                .HasForeignKey(p => p.StatusId)
                .HasConstraintName("FK_Payroll_Status")
                .OnDelete(DeleteBehavior.Restrict);

            // Employee who approved the payroll
            modelBuilder.Entity<Payroll>()
                .HasOne(p => p.ApprovedById)
                .WithMany()
                .HasForeignKey(p => p.ApprovedBy)
                .HasConstraintName("FK_Payroll_ApprovedBy")
                .OnDelete(DeleteBehavior.Restrict);

            // User role (HR/Admin) who paid the salary
            modelBuilder.Entity<Payroll>()
                .HasOne(p => p.PaidById)
                .WithMany()
                .HasForeignKey(p => p.PaidBy)
                .HasConstraintName("FK_Payroll_PaidBy")
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region PayrollDetail
            modelBuilder.Entity<PayrollDetail>()
                .HasKey(pd => pd.Id)
                .HasName("PK_PayrollDetail");

            // FK to Payroll
            modelBuilder.Entity<PayrollDetail>()
                .HasOne(pd => pd.Payroll)
                .WithMany(p => p.PayrollDetails)
                .HasForeignKey(pd => pd.PayrollId)
                .HasConstraintName("FK_PayrollDetail_Payroll")
                .OnDelete(DeleteBehavior.Cascade);

            // FK to TaxMaster
            modelBuilder.Entity<PayrollDetail>()
                .HasOne(pd => pd.Tax)
                .WithMany(t => t.PayrollDetails)
                .HasForeignKey(pd => pd.TaxId)
                .HasConstraintName("FK_PayrollDetail_TaxMaster")
                .OnDelete(DeleteBehavior.Restrict);

            // Column constraints
            modelBuilder.Entity<PayrollDetail>()
                .Property(pd => pd.ComponentName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<PayrollDetail>()
                .Property(pd => pd.ComponentType)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<PayrollDetail>()
                .Property(pd => pd.ComponentAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PayrollDetail>()
                .Property(pd => pd.Remarks)
                .HasMaxLength(250);
            #endregion

            #region UserAccount
            modelBuilder.Entity<UserAccount>()
                .HasKey(ua => ua.Id)
                .HasName("PK_UserAccount");

            // FK to Employee
            modelBuilder.Entity<UserAccount>()
                .HasOne(ua => ua.Employee)
                .WithOne(e => e.UserAccount)
                .HasForeignKey<UserAccount>(ua => ua.EmployeeId)
                .HasConstraintName("FK_UserAccount_Employee")
                .OnDelete(DeleteBehavior.Restrict);

            // FK to UserRole
            modelBuilder.Entity<UserAccount>()
                .HasOne(ua => ua.Role)
                .WithMany(ur => ur.UserAccounts)
                .HasForeignKey(ua => ua.UserRoleId)
                .HasConstraintName("FK_UserAccount_UserRole")
                .OnDelete(DeleteBehavior.Restrict);

            // Audit Trail (1 to many)
            modelBuilder.Entity<UserAccount>()
                .HasMany(ua => ua.AuditTrails)
                .WithOne(at => at.User)
                .HasForeignKey(at => at.UserId)
                .HasConstraintName("FK_AuditTrail_UserAccount")
                .OnDelete(DeleteBehavior.Cascade);

            // Notification Log (1 to many)
            modelBuilder.Entity<UserAccount>()
                .HasMany(ua => ua.NotificationLogs)
                .WithOne(nl => nl.User)
                .HasForeignKey(nl => nl.UserId)
                .HasConstraintName("FK_NotificationLog_UserAccount")
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Timesheet
            modelBuilder.Entity<Timesheet>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.EmployeeId)
                    .IsRequired();

                entity.Property(e => e.WorkDate)
                    .HasColumnType("date")
                    .IsRequired();

                entity.Property(e => e.HoursWorked)
                    .HasColumnType("decimal(5,2)")
                    .IsRequired();

                entity.Property(e => e.TaskDescription)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(e => e.IsBillable)
                    .IsRequired();

                entity.Property(e => e.StatusId)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()")
                    .IsRequired();

                entity.HasOne(e => e.Status)
                      .WithMany(s => s.Timesheets)
                      .HasForeignKey(e => e.StatusId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            #region TimesheetMaster
            
            modelBuilder.Entity<TimesheetStatusMaster>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.StatusName)
                      .HasMaxLength(100)
                      .IsRequired();
            });

            modelBuilder.Entity<TimesheetStatusMaster>().HasData(
                new TimesheetStatusMaster { Id = 1, StatusName = "Pending" },
                new TimesheetStatusMaster { Id = 2, StatusName = "Approved" },
                new TimesheetStatusMaster { Id = 3, StatusName = "Rejected" },
                new TimesheetStatusMaster { Id = 4, StatusName = "Submitted" }
            );
            #endregion
        }
    }
}
