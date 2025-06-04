using Esercizio15052025.Models;

namespace Esercizio20052025.Repository.User_Repo.Interfaces
{
    public interface IUser_Repo
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User entity);
        Task UpdateAsync(User entity);
        Task DeleteAsync(User entity);
        User? ReturnUserByName(string name);
        bool ExistsByNameBool(string name);

        User? ReturnUserDtoByName(string name);
    }
}
