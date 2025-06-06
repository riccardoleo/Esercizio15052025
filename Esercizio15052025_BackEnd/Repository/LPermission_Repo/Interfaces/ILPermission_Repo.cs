using Esercizio15052025.Models;

namespace Esercizio20052025.Repository.LPermission_Repo.Interfaces
{
    public interface ILPermission_Repo
    {
        Task<List<ListPermissionId>> GetAllAsync();
        Task<List<int>> GetPermissionIdsByUserIdAsync(int userId);
        Task AddAsync(ListPermissionId item);
        Task DeleteAsync(ListPermissionId item);
    }
}
