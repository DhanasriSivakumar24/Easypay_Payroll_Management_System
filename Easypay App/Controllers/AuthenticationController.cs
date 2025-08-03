using Easypay_App.Interface;
using Easypay_App.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easypay_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]
        public ActionResult<RegisterRequestDTO> Register(RegisterRequestDTO requestDTO)
        {
            try
            {
                var result = _authenticationService.Register(requestDTO);

                var response = new RegisterResponseDTO
                {
                    UserId = result.Id,
                    UserName = result.UserName
                };

                return Created("", response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Unable to add user: " + ex.Message);
            }
        }

        [HttpPost("Login")]
        public ActionResult<LoginResponseDTO> Login(LoginRequestDTO requestDTO)
        {
            try
            {
                var result = _authenticationService.Login(requestDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to Login"+ex.Message);
            }
        }
    }
}
