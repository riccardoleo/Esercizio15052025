using Esercizio15052025.DTO.PlantComponent_DTO;
using Esercizio15052025.DTO.ToolCategory_DTO;

namespace Esercizio15052025.Service.ToolsCategory_Service.Interfaces
{
    public interface IToolCategory_Service
    {
        Task<List<TC_DTO>> GetAllAsync(int index, int block);
        Task<TC_DTO?> GetByIdAsync(int id, int userID);
        Task AddAsync(TC_DTO entity);
        Task UpdateAsync(TC_DTO_Update entity);
        Task DeleteAsync(TC_DTO_Delete entity);
        Task<List<TC_DTO>> GetAllToolCategoriesByUserAsync(int userID, int index, int block);

    }
}
