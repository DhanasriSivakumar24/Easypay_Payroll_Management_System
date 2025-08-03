using Easypay_App.Models;
using Easypay_App.Models.DTO;

namespace Easypay_App.Interface
{
    public interface IAuthenticationService
    {
        LoginResponseDTO Login(LoginRequestDTO loginRequest);
        UserAccount Register(RegisterRequestDTO registerRequest);
    }
}
