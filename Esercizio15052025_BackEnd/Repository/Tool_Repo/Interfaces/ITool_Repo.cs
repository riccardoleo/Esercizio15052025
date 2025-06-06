using Esercizio15052025.Models;

namespace Esercizio15052025.Repository.Tool_Repo.Interfaces
{
    public interface ITool_Repo
    {
        Task<List<Tool>> GetAllAsync();
        Task<Tool?> GetByIdAsync(int id);
        Task AddAsync(Tool tool);
        Task UpdateAsync(Tool tool);
        Task DeleteAsync(Tool tool);
        bool ExistsByName(string name);
        Task<bool> IsToolOwnedByUserAsync(int toolId, int userID);
        Task<List<Tool>> GetAllToolsByUserAsync(int userID, int index, int block, List<int> permissionID);
    }
}
