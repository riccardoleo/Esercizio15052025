using Esercizio20052025.DTO.ListVisibility_DTO;

namespace Esercizio20052025.Service.LVisibility_Service.Interfaces
{
    public interface ILVisibility_Service
    {
        Task<LVisibilityResponse> GetAllAsync();
        Task<LVisibilityResponse?> GetByIdAsync(int id);
        Task<LVisibilityResponse> AddAsync(int UserID, int PermissionID);
        Task<LVisibilityResponse> DeleteAsync(ListVisibility_DTO item);
    }
}
