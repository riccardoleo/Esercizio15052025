using Esercizio15052025.Models;

namespace Esercizio20052025.Repository.LVisibility_Repo.Interfaces
{
    public interface ILVisibility_Repo
    {
        Task<List<ListVisibilityId>> GetAllAsync();
        Task<List<int>> GetPermissionIdsByUserIdAsync(int userId);
        Task AddAsync (ListVisibilityId item);
        Task DeleteAsync (ListVisibilityId item);
    }
}
