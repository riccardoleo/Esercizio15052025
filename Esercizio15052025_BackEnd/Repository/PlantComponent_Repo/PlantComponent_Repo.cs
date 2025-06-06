using Esercizio15052025.Models;
using Esercizio15052025.Repository.PlantComponent_Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esercizio15052025.Repository.PlantComponent_Repo
{
    public class PlantComponent_Repo : IPlantComponent_Repo
    {
        private readonly EQUIPPINGContext _context;

        public PlantComponent_Repo(EQUIPPINGContext context)
        {
            _context = context;
        }

        public async Task<List<PlantComponent>> GetAllAsync()
        {
            return await _context.PlantComponents.ToListAsync();
        }

        public async Task<List<PlantComponent>> GetPlantComponentsByUserAsync(int userID, int index, int block, List<int> permissionID)
        {
            List<PlantComponent> x = new();
            List<PlantComponent> plantComponents = new();

            permissionID.Add(userID);

            for (int i = 0; i < permissionID.Count; i++)
            {
                int j = permissionID[i];

                x = await _context.PlantComponents
                .Where(t => t.CreatedByUserId == j)
                .Skip((index - 1) * block)
                .Take(block)
                .ToListAsync();

                plantComponents.AddRange(x);
            }

            return plantComponents;
        }


        public async Task<bool> IsPlantComponentOwnedByUserAsync(int plantComponentId, int userID)
        {
            var plantcomponent = await _context.PlantComponents.FindAsync(plantComponentId);
            return plantcomponent != null && plantcomponent.CreatedByUserId == userID;
        }


        public async Task<PlantComponent?> GetByIdAsync(int id)
        {
            return await _context.PlantComponents.FindAsync(id);
        }

        public async Task AddAsync(PlantComponent entity)
        {
            _context.PlantComponents.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PlantComponent entity)
        {
            _context.PlantComponents.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PlantComponent entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public bool ExistsByName(string name)
        {
            return _context.PlantComponents.Any(c => c.Name == name);
        }


    }
}
