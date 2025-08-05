using AutoMapper;
using Azure.Core;
using Easypay_App.Exceptions;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;
using Easypay_App.Repositories;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Easypay_App.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IEmployeeService _employeeService;
        private readonly IRepository<string, UserAccount> _userRepository;
        private readonly IRepository<int, Employee> _employeeRepo;
        private readonly IRepository<int, UserRoleMaster> _userRoleMasterRepo;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthenticationService(IEmployeeService employeeService,
            IRepository<string, UserAccount> userRepository,
            IRepository<int,Employee> employeeRepo,
            IRepository<int, UserRoleMaster> userRoleMasterRepo,
            ITokenService tokenService,
            IMapper mapper)
        {
            _employeeService = employeeService;
            _userRepository = userRepository;
            _employeeRepo= employeeRepo;
            _userRoleMasterRepo = userRoleMasterRepo;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        #region LoginResponseDTO
        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest)
        {
            var User = await _userRepository.GetValueById(loginRequest.UserName);
            if (User == null)
                throw new NoItemFoundException();

            // Use the stored hash key to recreate HMAC
            HMACSHA256 hmacsha256 = new HMACSHA256(User.HashKey);
            var userPass = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.Password));
            for (int i = 0; i < userPass.Length; i++)
            {
                if (userPass[i] != User.Password[i])
                    throw new NoItemFoundException();
            }

            // Get role name from UserRoleMaster
            var role =  (await _userRoleMasterRepo.GetValueById(User.UserRoleId))?.UserRoleName ?? "Unknown";

            // Return token + username + role
            return new LoginResponseDTO
            {
                UserName = loginRequest.UserName,
                Role = role,
                Token = await _tokenService.GenerateToken(new LoginResponseDTO
                {
                    UserName = loginRequest.UserName,
                    Role = role
                })
            };
        }
        #endregion

        #region Register
        public async Task<UserAccount> Register(RegisterRequestDTO registerRequest)
        {
            try
            {
                UserAccount user = new UserAccount
                {
                    UserName = registerRequest.UserName,
                    EmployeeId = registerRequest.EmployeeId,
                    UserRoleId = registerRequest.RoleId,
                    ActiveFlag = true,
                    LastLogin = DateTime.Now
                };

                HMACSHA256 hmacsha256 = new HMACSHA256();
                user.HashKey = hmacsha256.Key;
                var defaultPassword = "#12" + registerRequest.UserName + "@12";
                user.Password = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(defaultPassword));

                return await _userRepository.AddValue(user);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Unable to add user");
            }
        }
        #endregion

    }
}
