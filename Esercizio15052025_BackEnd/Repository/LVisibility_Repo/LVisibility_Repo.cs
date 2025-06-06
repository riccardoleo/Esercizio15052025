using Esercizio15052025.Models;
using Esercizio20052025.Repository.LVisibility_Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esercizio20052025.Repository.LVisibility_Repo
{
    public class LVisibility_Repo(EQUIPPINGContext context) : ILVisibility_Repo
    {
        private readonly EQUIPPINGContext _context = context;

        public async Task<List<ListVisibilityId>> GetAllAsync()
        {
            return await _context.ListVisibilityIds.ToListAsync();
        }
        public async Task<List<int>> GetPermissionIdsByUserIdAsync(int userId)
        {
            return await _context.ListVisibilityIds
                                 .Where(x => x.UserId == userId)
                                 .Select(x => x.PermissionId)
                                 .ToListAsync();
        }

        public async Task AddAsync(ListVisibilityId item)
        {
            _context.ListVisibilityIds.AddRangeAsync(item);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(ListVisibilityId item)
        {
            _context.ListVisibilityIds.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
