using Esercizio15052025.DTO.Tool_DTO;

namespace Esercizio15052025.Service.Tool_Service.Interfeces
{
    public interface ITool_Service
    {
        Task<List<T_DTO>> GetAllAsync(int index, int block, int userID);
        Task<T_DTO?> GetByIdAsync(int id, int userID);
        Task AddAsync(T_DTO entity);
        Task UpdateAsync(T_DTO_Update entity);
        Task DeleteAsync(T_DTO_Delete entity);
        Task<List<T_DTO>> GetAllToolsByUserAsync(int userID, int index, int block);
    }
}
