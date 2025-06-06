using Esercizio20052025.DTO.ListPermission_DTO;

namespace Esercizio20052025.Service.LPermission_Service.Interfaces
{
    public interface ILPermission_Service
    {
       Task<LPermissionResponse> GetAllAsync();
       Task<LPermissionResponse?> GetByIdAsync(int id);
       Task<LPermissionResponse> AddAsync(int UserID, int PermissionID);
       Task<LPermissionResponse> DeleteAsync(ListPermission_DTO dto);
    }
}
