using Esercizio15052025.Models;
using Esercizio15052025.Repository.ToolCategory_Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esercizio15052025.Repository.ToolCategory_Repo
{
    public class ToolCategory_Repo : IToolCategory_Repo
    {
        private readonly EQUIPPINGContext _context;
        public ToolCategory_Repo(EQUIPPINGContext context)
        {
            _context = context;
        }

        public async Task<List<ToolCategory>> GetAllAsync()
        {
            return await _context.ToolCategories.ToListAsync();
        }
        public async Task<ToolCategory?> GetByIdAsync(int id)
        {
            return await _context.ToolCategories.FindAsync(id);
        }
        public async Task AddAsync(ToolCategory entity)
        {
            _context.ToolCategories.Add(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(ToolCategory entity)
        {
            _context.ToolCategories.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(ToolCategory entity)
        {
            _context.ToolCategories.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public bool ExistsByName(string name)
        {
            return _context.ToolCategories.Any(x => x.Name == name);
        }
        public async Task<bool> IsToolCategoryOwnedByUserAsync(int ToolCategoryId, int userID)
        {
            var toolCategory = await _context.ToolCategories.FindAsync(ToolCategoryId);
            return toolCategory != null && toolCategory.CreatedByUserId == userID;
        }
        public async Task<List<ToolCategory>> GetToolCategoriesByUserAsync(int userID, int index, int block)
        {
            return await _context.ToolCategories
                .Where(t => t.CreatedByUserId == userID)
                .Skip((index - 1) * block)
                .Take(block)
                .ToListAsync();
        }

    }
}
