using AutoMapper;
using Esercizio15052025.DTO.ToolCategory_DTO;
using Esercizio15052025.Models;
using Esercizio15052025.Repository.ToolCategory_Repo.Interfaces;
using Esercizio15052025.Service.ToolsCategory_Service.Interfaces;
using Esercizio15052025.Service.Check_Service;

namespace Esercizio15052025.Service.ToolsCategory_Service
{
    public class ToolCategory_Service : IToolCategory_Service
    {
        private readonly IToolCategory_Repo _repo;
        private readonly IMapper _mapper;

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public ToolCategory_Service(IToolCategory_Repo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<TC_DTO>> GetAllAsync(int index, int block)
        {
            if (index == 0 || block == 0)
            {
                Logger.Error("0 non e' un numero valido");
                throw new ArgumentException("0 non e' un numero valido");
            }

            var entity = await _repo.GetAllAsync();

            var result = entity.Skip((index - 1) * block).Take(block).ToList();

            return _mapper.Map<List<TC_DTO>>(result);
        }

        public async Task<List<TC_DTO>> GetAllToolCategoriesByUserAsync(int userID, int index, int block)
        {
            if (index <= 0 || block <= 0)
            {
                Logger.Error("Index e block devono essere > 0");
                throw new InvalidOperationException("Index e block non validi");
            }

            var entity = await _repo.GetToolCategoriesByUserAsync(userID, index, block);

            return _mapper.Map<List<TC_DTO>>(entity);       
        }

        public async Task<TC_DTO?> GetByIdAsync(int id, int userID)
        {
            if (!await _repo.IsToolCategoryOwnedByUserAsync(id, userID))
            {
                Logger.Error("Il tool non e' associato a questo utente");
                throw new InvalidOperationException("Tool non disponibile");
            }

            var entity = await _repo.GetByIdAsync(id);

            if (entity == null)
            {
                Logger.Error("L'Id inserito non e' valido");
                return null;
            }

            return _mapper.Map<TC_DTO>(entity);
        }
        public Task AddAsync(TC_DTO dto)
        {
            try
            {
                Check_if_Null.CheckString(dto.Name);
                //Check_if_Null.CheckString(dto.CreatedByUserName);

                var entity = _mapper.Map<ToolCategory>(dto);
                return _repo.AddAsync(entity);

            }catch (Exception ex)
            {
                Logger.Error(ex , "Qualcosa e' andato storto {}", ex.Message);
                throw;
            }
        }
        public Task UpdateAsync(TC_DTO_Update dto)
        {
            Check_if_Null.CheckString(dto.Name);
            Check_if_Null.CheckInt(dto.CategoryId);

            var entity = _mapper.Map<ToolCategory>(dto);
            return _repo.UpdateAsync(entity);
        }
        public Task DeleteAsync(TC_DTO_Delete dto)
        {
            Check_if_Null.CheckInt(dto.CategoryId);

            var entity = _mapper.Map<ToolCategory>(dto);
            return _repo.DeleteAsync(entity);
        }
    }
}
