using AutoMapper;
using Easypay_App.Context;
using Easypay_App.Interfaces;
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

            builder.Services.AddScoped<IRepository<int, Employee>, EmployeeRepositoryDb>();
            builder.Services.AddScoped<IRepository<int, DepartmentMaster>, DepartmentRepository>();
            builder.Services.AddScoped<IRepository<int, RoleMaster>, RoleRepository>();
            builder.Services.AddScoped<IRepository<int, EmployeeStatusMaster>, EmployeeStatusRepository>();
            builder.Services.AddScoped<IRepository<int, UserRoleMaster>, UserRoleRepository>();

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
