namespace Esercizio20052025.DTO.Users_DTO
{
    public class User_DTO
    {
        public int? UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string? Role { get; set; }
        public string? passwdRole { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
