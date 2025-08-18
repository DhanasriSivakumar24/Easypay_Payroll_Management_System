using Easypay_App.Models;
using Easypay_App.Models.DTO;

namespace Easypay_App.Interface
{
    public interface IAuthenticationService
    {
        public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest);
        public Task<UserAccount> Register(RegisterRequestDTO registerRequest);
    }
}
