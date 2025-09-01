using AutoMapper;
using Easypay_App.Context;
using Easypay_App.Exceptions;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;
using Easypay_App.Repositories;
using Easypay_App.Services;
using EasyPay_App.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Easypay_Test
{
    public class AuthenticationServiceTest
    {
        private Mock<IEmployeeService> _employeeService;
        private IRepository<string, UserAccount> _userRepo;
        private IRepository<int, Employee> _employeeRepo;
        private IRepository<int, UserRoleMaster> _roleRepo;
        private Mock<ITokenService> _tokenService;
        private Mock<IMapper> _mockMapper;
        private AuthenticationService _authService;
        private PayrollContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PayrollContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new PayrollContext(options);
            _userRepo = new UserRepository(_context);
            _employeeRepo = new EmployeeRepository(_context);
            _roleRepo = new UserRoleRepository(_context);

            _employeeService = new Mock<IEmployeeService>();
            _tokenService = new Mock<ITokenService>();
            _mockMapper = new Mock<IMapper>();

            _authService = new AuthenticationService(
                _employeeService.Object,
                _userRepo,
                _employeeRepo,
                _roleRepo,
                _tokenService.Object,
                _mockMapper.Object
            );
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Register_ShouldCreateUser_WhenValid()
        {
            await _employeeRepo.AddValue(new Employee { Id = 1, FirstName = "Test", LastName = "User", UserRoleId = 3 });
            await _roleRepo.AddValue(new UserRoleMaster { Id = 3, UserRoleName = "HR Manager" });

            var request = new RegisterRequestDTO { UserName = "testuser", EmployeeId = 1 };

            var result = await _authService.Register(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserName, Is.EqualTo("testuser"));
            Assert.That(result.UserRoleId, Is.EqualTo(3));
            Assert.That(result.Password, Is.Not.Null);
            Assert.That(result.HashKey, Is.Not.Null);
        }

        [Test]
        public async Task Login_ShouldReturnToken_WhenValidCredentials()
        {
            var password = "testpassword";
            using var hmac = new HMACSHA256();
            var hashKey = hmac.Key;
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            await _userRepo.AddValue(new UserAccount
            {
                UserName = "testuser",
                Password = passwordHash,
                HashKey = hashKey,
                UserRoleId = 1
            });
            await _roleRepo.AddValue(new UserRoleMaster { Id = 1, UserRoleName = "Admin" });

            _tokenService.Setup(t => t.GenerateToken(It.IsAny<LoginResponseDTO>())).ReturnsAsync("fake-jwt-token");

            var request = new LoginRequestDTO { UserName = "testuser", Password = password };
            var result = await _authService.Login(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserName, Is.EqualTo("testuser"));
            Assert.That(result.Token, Is.EqualTo("fake-jwt-token"));
            Assert.That(result.Role, Is.EqualTo("Admin"));
        }

        [Test]
        public void Login_ShouldThrow_WhenInvalidUsername()
        {
            var request = new LoginRequestDTO { UserName = "nonexistent", Password = "wrong" };
            Assert.ThrowsAsync<Exception>(async () => await _authService.Login(request));
        }

        [Test]
        public void Login_ShouldThrow_WhenInvalidPassword()
        {
            var password = "correct";
            using var hmac = new HMACSHA256();
            var hashKey = hmac.Key;
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            _userRepo.AddValue(new UserAccount { UserName = "testuser", Password = passwordHash, HashKey = hashKey, UserRoleId = 1 }).Wait();

            var request = new LoginRequestDTO { UserName = "testuser", Password = "wrong" };
            Assert.ThrowsAsync<Exception>(async () => await _authService.Login(request));
        }
    }
}
