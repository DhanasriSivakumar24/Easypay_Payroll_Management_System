using Easypay_App.Filters;
using Easypay_App.Interface;
using Easypay_App.Models;
using Easypay_App.Models.DTO;
using Easypay_App.Services;
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
        private readonly IAuditTrailService _auditTrailService;

        public AuthenticationController(IAuthenticationService authenticationService, IAuditTrailService auditTrailService)
        {
            _authenticationService = authenticationService;
            _auditTrailService = auditTrailService;
        }

        [HttpPost("Register")]
        [Authorize(Roles = "Admin, HR Manager")]
        public async Task<ActionResult<RegisterResponseDTO>> Register(
                                    [FromBody] RegisterRequestDTO requestDTO, IRepository<int, UserRoleMaster> roleRepo)
        {
            var result = await _authenticationService.Register(requestDTO);

            var role = await roleRepo.GetValueById(result.UserRoleId);
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var response = new RegisterResponseDTO
            {
                UserId = result.Id,
                UserName = result.UserName,
                Role = role?.UserRoleName ?? "Unknown"
            };

            await _auditTrailService.LogAction(
                   User.Identity.Name,
                   actionId: 26,
                   entityName: "New User Register",
                   entityId: result.Id,
                   oldValue: "N/A",
                   newValue: result,
                   ipAddress: ipAddress
               );
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseDTO>> Login(LoginRequestDTO requestDTO)
        {
            var result = await _authenticationService.Login(requestDTO);

            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            await _auditTrailService.LogAction(
                   User.Identity.Name,
                   actionId: 4,
                   entityName: "Login",
                   entityId: 0,
                   oldValue: "N/A",
                   newValue: "N/A",
                   ipAddress: ipAddress
               );

            return Ok(result);
        }
    }
}
