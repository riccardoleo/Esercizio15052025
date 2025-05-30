using Esercizio15052025.Models;

namespace Esercizio15052025.Repository.ToolCategory_Repo.Interfaces
{
    public interface IToolCategory_Repo
    {
        Task<List<ToolCategory>> GetAllAsync();
        Task<ToolCategory?> GetByIdAsync(int id);
        Task AddAsync(ToolCategory entity);
        Task UpdateAsync(ToolCategory entity);
        Task DeleteAsync(ToolCategory entity);
        bool ExistsByName(string name);
        Task<bool> IsToolCategoryOwnedByUserAsync(int ToolCategoryId, int userID);
        Task<List<ToolCategory>> GetToolCategoriesByUserAsync(int userID, int index, int block);

    }
}
