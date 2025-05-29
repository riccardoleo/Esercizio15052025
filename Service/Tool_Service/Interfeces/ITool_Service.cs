using Esercizio15052025.DTO.Tool_DTO;
using Esercizio20052025.DTO.Tool_DTO;

namespace Esercizio15052025.Service.Tool_Service.Interfeces
{
    public interface ITool_Service
    {
        Task<ToolDTO_Response> GetAllAsync(int index, int block, int userID);
        Task<ToolDTO_Response?> GetByIdAsync(int id, int userID);
        Task<ToolDTO_Response> AddAsync(T_DTO entity);
        Task<ToolDTO_Response> UpdateAsync(T_DTO_Update entity);
        Task<ToolDTO_Response> DeleteAsync(T_DTO_Delete entity);
        Task<List<ToolDTO_Response>> GetAllToolsByUserAsync(int userID, int index, int block);
    }
}
