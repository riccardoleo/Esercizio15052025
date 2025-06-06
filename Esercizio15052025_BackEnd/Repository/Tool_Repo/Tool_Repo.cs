using Esercizio15052025.Models;
using Esercizio15052025.Repository.Tool_Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esercizio15052025.Repository.Tool_Repo
{
    public class Tool_Repo(EQUIPPINGContext context) : ITool_Repo
    {
        private readonly EQUIPPINGContext _context = context;

        public async Task<List<Tool>> GetAllAsync()
        {
            return await _context.Tools.ToListAsync();
        }

        public async Task<List<Tool>> GetAllToolsByUserAsync(int userID, int index, int block, List<int> permissionID)
        {
            List<Tool> x = new List<Tool>();
            List<Tool> tools = new List<Tool>();

            permissionID.Add(userID);

            for (int i = 0; i < permissionID.Count; i++)
            {
                int j = permissionID[i];

                x = await _context.Tools
                .Where(t => t.CreatedByUserId == j)
                .Skip((index - 1) * block)
                .Take(block)
                .ToListAsync();

                tools.AddRange(x);
            }

            return tools;
        }

        public async Task<Tool?> GetByIdAsync(int id)
        {
            return await _context.Tools.FindAsync(id);
        }

        public async Task AddAsync(Tool tool)
        {
            _context.Tools.Add(tool);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tool tool)
        {
            _context.Tools.Update(tool);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Tool tool)
        {
            _context.Tools.Remove(tool);
            await _context.SaveChangesAsync();
        }

        public bool ExistsByName(string name)
        {
            return _context.Tools.Any(t => t.Name == name);
        }

        public async Task<bool> IsToolOwnedByUserAsync(int toolId, int userID)
        {
            var tool = await _context.Tools.FindAsync(toolId);
            return tool != null && tool.CreatedByUserId == userID;
        }
    }
}
