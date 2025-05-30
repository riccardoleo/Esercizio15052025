using Esercizio15052025.DTO.PlantComponent_DTO;
using Esercizio15052025.DTO.ToolCategory_DTO;

namespace Esercizio15052025.Service.ToolsCategory_Service.Interfaces
{
    public interface IToolCategory_Service
    {
        Task<ToolCategoryDTO_Response> GetAllAsync(int index, int block);
        Task<ToolCategoryDTO_Response?> GetByIdAsync(int id, int userID);
        Task<ToolCategoryDTO_Response> AddAsync(TC_DTO entity);
        Task<ToolCategoryDTO_Response> UpdateAsync(TC_DTO_Update entity);
        Task<ToolCategoryDTO_Response> DeleteAsync(TC_DTO_Delete entity);
        Task<ToolCategoryDTO_Response> GetAllToolCategoriesByUserAsync(int userID, int index, int block);

    }
}
