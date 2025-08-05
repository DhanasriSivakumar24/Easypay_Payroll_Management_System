using Easypay_App.Filters;
using Easypay_App.Interface;
using Easypay_App.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easypay_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomExceptionFilter]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RegisterResponseDTO>> Register(RegisterRequestDTO requestDTO)
        {
            var result = await _authenticationService.Register(requestDTO);

            var response = new RegisterResponseDTO
            {
                UserId = result.Id,
                UserName = result.UserName
            };

            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseDTO>> Login(LoginRequestDTO requestDTO)
        {
            var result = await _authenticationService.Login(requestDTO);
            return Ok(result);
        }
    }
}
