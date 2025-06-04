using Esercizio15052025.Models;

namespace Esercizio20052025.Repository.LPermission_Repo.Interfaces
{
    public interface ILPermission_Repo
    {
        Task<List<ListPermissionId>> GetAllAsync();
        Task<ListPermissionId?> GetByIdAsync(int id);
        Task AddAsync(ListPermissionId item);
        Task DeleteAsync(ListPermissionId item);
    }
}
