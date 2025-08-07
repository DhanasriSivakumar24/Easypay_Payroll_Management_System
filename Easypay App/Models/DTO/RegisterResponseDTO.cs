namespace Easypay_App.Models.DTO
{
    public class RegisterResponseDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
