using Easypay_App.Models.DTO;
using Microsoft.AspNetCore.Identity.Data;

namespace Easypay_App.Interface
{
    public interface ITokenService
    {
        public string GenerateToken(LoginResponseDTO login);
    }
}
