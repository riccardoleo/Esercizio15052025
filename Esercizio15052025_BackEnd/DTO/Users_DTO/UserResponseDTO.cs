namespace Esercizio20052025.DTO.Users_DTO
{
    public class UserResponseDTO
    {
        /// <summary>
        /// [200] OK / [204] No Content / [404] Not found / [500] Internal Server Error
        /// </summary>
        public int success;
        public int? UserId;
        public User_DTO? user_DTO;
        public List<User_DTO>? users;
        public string? token;
        public string? message;
        public string? UserRole;
        public bool? IsAdmin;
    }
}
