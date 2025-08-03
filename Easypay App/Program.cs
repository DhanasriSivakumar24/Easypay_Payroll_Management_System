using AutoMapper;
using Easypay_App.Context;
using Easypay_App.Interface;
using Easypay_App.Mapper;
using Easypay_App.Models;
using Easypay_App.Repositories;
using Easypay_App.Services;
using EasyPay_App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Easypay_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<PayrollContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IPayrollPolicyService,PayrollPolicyService>();
            builder.Services.AddScoped<IBenefitEnrollmentService, BenefitEnrollmentService>();
            builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
            builder.Services.AddScoped<IPayrollService, PayrollService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<ITokenService, TokenService>();

            builder.Services.AddScoped<IRepository<int, Employee>, EmployeeRepositoryDb>();
            builder.Services.AddScoped<IRepository<int, DepartmentMaster>, DepartmentRepository>();
            builder.Services.AddScoped<IRepository<int, RoleMaster>, RoleRepository>();
            builder.Services.AddScoped<IRepository<int, EmployeeStatusMaster>, EmployeeStatusRepository>();
            builder.Services.AddScoped<IRepository<int, UserRoleMaster>, UserRoleRepository>();
            builder.Services.AddScoped<IRepository<int,PayrollPolicyMaster>,PayrollPolicyRepository>();
            builder.Services.AddScoped<IRepository<int,BenefitEnrollment>, BenefitEnrollmentRepository>();
            builder.Services.AddScoped<IRepository<int, BenefitStatusMaster>, BenefitsStatusRepository>();
            builder.Services.AddScoped<IRepository<int, BenefitMaster>, BenefitsRepository>();
            builder.Services.AddScoped<IRepository<int, LeaveStatusMaster>, LeaveStatusRepository>();
            builder.Services.AddScoped<IRepository<int, LeaveTypeMaster>, LeaveTypeRepository>();
            builder.Services.AddScoped<IRepository<int, PayrollStatusMaster>, PayrollStatusRepository>();
            builder.Services.AddScoped<IRepository<int, PayrollDetail>, PayrollDetailRepository>();
            builder.Services.AddScoped<IRepository<int, Payroll>, PayrollRepository>();
            builder.Services.AddScoped<IRepository<int, LeaveRequest>, LeaveRequestRepository>();
            builder.Services.AddScoped<IRepository<string, UserAccount>, UserRepository>();


            builder.Services.AddAutoMapper(typeof(EmployeeMapper));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
