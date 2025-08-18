using Easypay_App.Filters;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easypay_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomExceptionFilter]
    [EnableCors("DefaultCORS")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpPost("Register")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<ActionResult<RegisterResponseDTO>> Register(
                                    [FromBody] RegisterRequestDTO requestDTO, IRepository<int, UserRoleMaster> roleRepo)
        {
            var result = await _authenticationService.Register(requestDTO);

            var role = await roleRepo.GetValueById(result.UserRoleId);

            var response = new RegisterResponseDTO
            {
                UserId = result.Id,
                UserName = result.UserName,
                Role = role?.UserRoleName ?? "Unknown"
            };

            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseDTO>> Login(LoginRequestDTO requestDTO)
        {
            var result = await _authenticationService.Login(requestDTO);
            return Ok(result);
        }
    }
}
