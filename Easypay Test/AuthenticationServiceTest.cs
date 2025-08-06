using AutoMapper;
using Easypay_App.Exceptions;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;
using Easypay_App.Repositories;
using Easypay_App.Services;
using EasyPay_App.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Cryptography;
using System.Text;

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

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new Easypay_App.Context.PayrollContext(options);
            _userRepo = new UserRepository(context);
            _employeeRepo = new EmployeeRepository(context);
            _roleRepo = new UserRoleRepository(context);

            _employeeService = new Mock<IEmployeeService>();
            _tokenService = new Mock<ITokenService>();
            _mockMapper = new Mock<IMapper>();

            _authService = new AuthenticationService(
                _employeeService.Object,
                _userRepo,
                _employeeRepo,
                _roleRepo,
                _tokenService.Object,
                _mockMapper.Object);
        }

        #region Register
        [Test]
        public async Task Register()
        {
            var request = new RegisterRequestDTO
            {
                UserName = "testuser",
                EmployeeId = 1,
                RoleId = 3
            };

            var result = await _authService.Register(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserName, Is.EqualTo("testuser"));
            Assert.That(result.UserRoleId, Is.EqualTo(3));
        }
        #endregion

        #region Login_Success
        [Test]
        public async Task Login_ValidCredentials()
        {
            var password = "testpassword";
            var hmac = new HMACSHA256();
            var hashKey = hmac.Key;
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            var user = new UserAccount
            {
                UserName = "testuser",
                Password = passwordHash,
                HashKey = hashKey,
                UserRoleId = 1
            };

            await _userRepo.AddValue(user);
            await _roleRepo.AddValue(new UserRoleMaster { Id = 1, UserRoleName = "Admin" });

            _tokenService.Setup(t => t.GenerateToken(It.IsAny<LoginResponseDTO>())).ReturnsAsync("fake-jwt-token");

            var loginRequest = new LoginRequestDTO
            {
                UserName = "testuser",
                Password = password
            };

            var result = await _authService.Login(loginRequest);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserName, Is.EqualTo("testuser"));
            Assert.That(result.Token, Is.EqualTo("fake-jwt-token"));
            Assert.That(result.Role, Is.EqualTo("Admin"));
        }
        #endregion

        #region Login_InvalidUsername
        [Test]
        public void Login_InvalidUsername()
        {
            var request = new LoginRequestDTO
            {
                UserName = "nonexistent",
                Password = "wrong"
            };

            Assert.ThrowsAsync<NoItemFoundException>(async () => await _authService.Login(request));
        }
        #endregion

        #region Login_InvalidPassword
        [Test]
        public async Task Login_InvalidPassword_ThrowException()
        {
            var password = "correct";
            var hmac = new HMACSHA256();
            var hashKey = hmac.Key;
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            var user = new UserAccount
            {
                UserName = "testuser",
                Password = passwordHash,
                HashKey = hashKey,
                UserRoleId = 1
            };

            await _userRepo.AddValue(user);

            var request = new LoginRequestDTO
            {
                UserName = "testuser",
                Password = "wrong"
            };

            Assert.ThrowsAsync<NoItemFoundException>(async () => await _authService.Login(request));
        }
        #endregion
    }
}
