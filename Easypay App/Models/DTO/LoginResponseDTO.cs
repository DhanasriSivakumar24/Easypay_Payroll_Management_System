namespace Easypay_App.Models.DTO
{
    public class LoginResponseDTO
    {
        public int EmployeeId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
