using Esercizio20052025.DTO.Users_DTO;

namespace Esercizio20052025.Service.User_Service.Interfaces
{
    public interface IUser_Service
    {
        Task<UserResponseDTO> GetAllAsync(int index, int block);
        Task<UserResponseDTO> GetByIdAsync(int id);
        Task<UserResponseDTO> RegisterAsync(User_DTO entity);
        Task<UserResponseDTO> UpdateAsync(User_DTO entity);
        Task<UserResponseDTO> DeleteAsync(UserCredential_DTO entity);
        UserResponseDTO Login(UserCredential_DTO dto);
        UserResponseDTO UserIDFromUserName(string username);
        UserResponseDTO CheckRole(String userRole, String userName);
    }
}
