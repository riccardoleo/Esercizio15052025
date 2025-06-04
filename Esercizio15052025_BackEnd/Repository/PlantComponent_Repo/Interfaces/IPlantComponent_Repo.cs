using Esercizio15052025.DTO.PlantComponent_DTO;
using Esercizio15052025.Models;

namespace Esercizio15052025.Repository.PlantComponent_Repo.Interfaces
{
    public interface IPlantComponent_Repo
    {
        Task<List<PlantComponent>> GetAllAsync();
        Task<PlantComponent?> GetByIdAsync(int id);
        Task AddAsync(PlantComponent entity);
        Task UpdateAsync(PlantComponent entity);
        Task DeleteAsync(PlantComponent entity);
        bool ExistsByName(string name);
        Task<bool> IsPlantComponentOwnedByUserAsync(int plantComponentId, int userID);
        Task<List<PlantComponent>> GetPlantComponentsByUserAsync(int userID, int index, int block, List<int> permissionID);


    }
}
