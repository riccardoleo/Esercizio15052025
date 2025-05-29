using AutoMapper;
using Esercizio15052025.DTO.Tool_DTO;
using Esercizio15052025.Models;
using Esercizio15052025.Repository.Tool_Repo.Interfaces;
using Esercizio15052025.Service.Check_Service;
using Esercizio15052025.Service.Tool_Service.Interfeces;

namespace Esercizio15052025.Service.Tool_Service
{
    public class Tool_Service(ITool_Repo repo, IMapper mapper) : ITool_Service
    {
        private readonly ITool_Repo _repo = repo;
        private readonly IMapper _mapper = mapper;


        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public async Task<List<T_DTO>> GetAllAsync(int index, int block, int userID)
        {

            if (index == 0 || block == 0)
            {
                Logger.Error("0 non e' un numero valido");
                throw new InvalidOperationException("0 non e' un numero valido");
            }

            var entity = await _repo.GetAllAsync();

            var result = entity.Skip((index - 1) * block).Take(block).ToList();

            return _mapper.Map<List<T_DTO>>(result);
        }

        public async Task<List<T_DTO>> GetAllToolsByUserAsync(int userID, int index, int block)
        {
            if (index <= 0 || block <= 0)
            {
                Logger.Error("Index e block devono essere > 0");
                throw new InvalidOperationException("Index e block non validi");
            }

            var entities = await _repo.GetAllToolsByUserAsync(userID, index, block);

            return _mapper.Map<List<T_DTO>>(entities);
        }
        public async Task<T_DTO?> GetByIdAsync(int id, int userID)
        {
            if (!await _repo.IsToolOwnedByUserAsync(id, userID))
            {
                Logger.Error("Il tool non e' associato a questo utente");
                throw new InvalidOperationException("Tool non disponibile");
            }

            var entity = await _repo.GetByIdAsync(id);

            if (entity == null)
            {
                Logger.Error("L'Id inserito non e' valido");
                throw new InvalidOperationException("L'Id inserito non e' valido");
            }

            return _mapper.Map<T_DTO>(entity);
        }

        public async Task AddAsync(T_DTO dto)
        {
            Check_if_Null.CheckString(dto.Name);
            //Check_if_Null.CheckString(dto.CreatedByUserId);

            if (_repo.ExistsByName(dto.Name))
            {
                Logger.Error("Il nome e' gia' esistente");
                throw new InvalidOperationException("Il nome è già esistente");
            }

            if (dto.CreationDate == DateTime.MinValue)
                dto.CreationDate = DateTime.Now;

            var entity = _mapper.Map<Tool>(dto);
            await _repo.AddAsync(entity);
        }

        public Task UpdateAsync(T_DTO_Update dto)
        {
            Check_if_Null.CheckString(dto.Name);
            Check_if_Null.CheckInt(dto.ToolId);
            //Check_if_Null.CheckString(dto.CreatedByUserName);

            var entity = _mapper.Map<Tool>(dto);
            return _repo.UpdateAsync(entity);
        }

        public Task DeleteAsync(T_DTO_Delete dto)
        {
            Check_if_Null.CheckInt(dto.ToolId);

            var entity = _mapper.Map<Tool>(dto);
            return _repo.DeleteAsync(entity);
        }


    }
}
