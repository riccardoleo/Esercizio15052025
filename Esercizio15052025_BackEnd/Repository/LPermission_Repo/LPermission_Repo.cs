using Esercizio15052025.Models;
using Esercizio20052025.Repository.LPermission_Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esercizio20052025.Repository.LPermission_Repo
{
    public class LPermission_Repo(EQUIPPINGContext _context) : ILPermission_Repo
    {
        private readonly EQUIPPINGContext _context;

        public async Task<List<ListPermissionId>> GetAllAsync()
        {
            return await _context.ListPermissionIds.ToListAsync();
        }
        public async Task<ListPermissionId?> GetByIdAsync(int id)
        {
            return await _context.ListPermissionIds.FindAsync(id);
        }
        public async Task AddAsync(ListPermissionId item)
        {
            await _context.ListPermissionIds.AddRangeAsync(item);
        }
        public async Task DeleteAsync(ListPermissionId item)
        {
            _context.ListPermissionIds.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
