namespace Esercizio20052025.DTO.Users_DTO
{
    public class UserResponseDTO
    {
        /// <summary>
        /// [200] OK / [204] No Content / [404] Not found / [500] Internal Server Error
        /// </summary>
        public int success {  get; set; }
        public int? UserId { get; set; }
        public User_DTO? user_DTO { get; set; }
        public List<User_DTO>? users { get; set; }
        public string? token { get; set; }
        public string? message { get; set; }
        public string? UserRole { get; set; }
        public bool? IsAdmin { get; set; }
        public string UserName { get; set; }
    }
}
