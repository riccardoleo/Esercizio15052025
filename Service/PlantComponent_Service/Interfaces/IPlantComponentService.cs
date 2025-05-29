using Esercizio15052025.DTO.PlantComponent_DTO;
using Esercizio15052025.DTO.Tool_DTO;
using Esercizio20052025.DTO.PlantComponent_DTO;

namespace Esercizio15052025.Service.PlantComponent_Service.Interfaces
{
    public interface IPlantComponentService
    {
        Task<PlantComponent_Response> GetAllAsync(int index, int blocco);
        Task<PlantComponent_Response> GetByIdAsync(int id, int userID);
        Task<PlantComponent_Response> AddAsync(PC_DTO entity);
        Task<PlantComponent_Response> UpdateAsync(PC_DTO_Update entity);
        Task<PlantComponent_Response> DeleteAsync(PC_DTO_Delete entity);
        Task<PlantComponent_Response> GetAllPlantComponentsByUserAsync(int userID, int index, int block);

    }
}
