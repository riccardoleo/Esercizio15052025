using AutoMapper;
using Esercizio15052025.Models;
using Esercizio20052025.DTO.ListVisibility_DTO;
using Esercizio20052025.Repository.LVisibility_Repo.Interfaces;
using Esercizio20052025.Service.LVisibility_Service.Interfaces;

namespace Esercizio20052025.Service.LVisibility_Service
{
    public class LVisibility_Service(ILVisibility_Repo repo, IMapper mapper) : ILVisibility_Service
    {
        private readonly ILVisibility_Repo _repo = repo;
        private readonly IMapper _mapper = mapper;

        public async Task<LVisibilityResponse> GetAllAsync()
        {
            LVisibilityResponse response = new();

            List<ListVisibilityId> listVisibility = _repo.GetAllAsync().Result;

            if(listVisibility.Count == 0)
            {
                response.success = 404;
                response.message = "nessun elemento trovato";
                return response;
            }

            response.listVisibility_DTOs = _mapper.Map<List<ListVisibility_DTO>>(listVisibility);
            response.success = 200;
            response.message = "Lista ottenuta con successo";
            return response;
        }

        public async Task<LVisibilityResponse?> GetByIdAsync(int id)
        {
            LVisibilityResponse dto = new();

            if (id == 0)
            {
                dto.success = 204;
                dto.message = "id inserito non valido";
                return dto;
            }

            List<int> permissionIds = await _repo.GetPermissionIdsByUserIdAsync(id);

            if(permissionIds.Count == 0)
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

        public async Task<LVisibilityResponse> AddAsync(int UserID, int PermissionID)
        {
            LVisibilityResponse response = new();
            ListVisibilityId item = new();
            ListVisibility_DTO dto = new ListVisibility_DTO();

            dto.UserId = UserID;
            dto.PermissionId = PermissionID;

            item = _mapper.Map <ListVisibilityId> (dto);

            await _repo.AddAsync(item);
             
            response.success = 200;
            response.message = "L'utente con l'ID "+ item.UserId + " ha la possibilitia' di vedere gli elementi associato all'ID " + dto.PermissionId;
            return response;
        }

        public async Task<LVisibilityResponse> DeleteAsync(ListVisibility_DTO dto)
        {
            LVisibilityResponse response = new();
            ListVisibilityId item = new();

            item = _mapper.Map<ListVisibilityId>(dto);

            await _repo.DeleteAsync(item);

            response.success = 200;
            response.message = "Eliminato il permesso";
            return response;
        }
    }
}
