using AutoMapper;
using Azure;
using Esercizio15052025.Models;
using Esercizio20052025.DTO.ListPermission_DTO;
using Esercizio20052025.Repository.LPermission_Repo.Interfaces;
using Esercizio20052025.Service.LPermission_Service.Interfaces;

namespace Esercizio20052025.Service.LPermission_Service
{
    public class LPermission_Service(ILPermission_Repo repo, IMapper mapper) : ILPermission_Service
    {
        private readonly IMapper _mapper = mapper;
        private readonly ILPermission_Repo _repo = repo;
        public async Task<LPermissionResponse> GetAllAsync()
        {
            LPermissionResponse dto = new();

            List<ListPermissionId> listPermissionIds = _repo.GetAllAsync().Result;

            if (listPermissionIds.Count == 0)
            {
                dto.success = 404;
                dto.message = "nessun elemento trovato";
                return dto;
            }

            dto.listPermission_DTOs = _mapper.Map<List<ListPermission_DTO>>(listPermissionIds);
            dto.success = 200;
            dto.message = "Lista ottenuta con successo";
            return dto;
        }

        public async Task<LPermissionResponse?> GetByIdAsync(int id)
        {
            LPermissionResponse dto = new();

            if (id == 0 || id == null)
            {
                dto.success = 204;
                dto.message = "id inserito non valido";
                return dto;
            }

            List<int> permissionIds = await _repo.GetPermissionIdsByUserIdAsync(id);

            if (permissionIds.Count == 0)
            {
                dto.success = 404;
                dto.message = "nessun permessionId trovato";
                return dto;
            }

            dto.PermissionIdList = permissionIds;
            dto.success = 200;
            dto.message = "Lista ottenuta con successo";

            return dto;
        }
        public async Task<LPermissionResponse> AddAsync(int UserID, int PermissionID)
        {
            LPermissionResponse response = new();
            ListPermissionId item = new ListPermissionId();
            ListPermission_DTO dto = new ListPermission_DTO();

            dto.UserId = UserID;
            dto.PermissionId = PermissionID;

            item = _mapper.Map<ListPermissionId>(dto);

            await _repo.AddAsync(item);

            response.success = 200;
            response.message = "L'utente con l'ID " + item.UserId + " ha la possibilitia' di vedere gli elementi associato all'ID " + dto.PermissionId;
            return response;
        }

        public async Task<LPermissionResponse> DeleteAsync(ListPermission_DTO dto)
        {
            LPermissionResponse response = new();
            ListPermissionId item = new();

            item = _mapper.Map<ListPermissionId>(dto);

            await _repo.DeleteAsync(item);

            response.success = 200;
            response.message = "Eliminato il permesso";
            return response;
        }
    }
}
