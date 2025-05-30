using AutoMapper;
using Esercizio15052025.DTO.ToolCategory_DTO;
using Esercizio15052025.Models;
using Esercizio15052025.Repository.ToolCategory_Repo.Interfaces;
using Esercizio15052025.Service.ToolsCategory_Service.Interfaces;
using Esercizio15052025.Service.Check_Service;
using Microsoft.IdentityModel.Tokens;

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

        /// <summary>
        /// [TC01] Questo servizio ritorna un oggetto contenente una List ToolCategory_DTO con tutti gli toolCategory esistenti
        /// </summary>
        /// <param name="index"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        public async Task<ToolCategoryDTO_Response> GetAllAsync(int index, int block)
        {
            ToolCategoryDTO_Response result = new ToolCategoryDTO_Response();

            if (index == 0 || block == 0)
            {
                Logger.Error("[TC01A1] 0 non e' un numero valido");
                result.success = 0;
                result.message = ("[TC01A1] 🚠🥀 0 non e' un numero valido");
                return result;
            }

            List<ToolCategory> entity = await _repo.GetAllAsync();

            if (entity.Count == 0)
            {
                Logger.Error("[TC01A2] nessun Tool Category trovato");
                result.success = 404;
                result.message = ("[TC01A2] 💔 nessun Tool Category trovato");
                return result;
            }

            result.toolCategories = _mapper.Map<List<TC_DTO>>(entity.Skip((index - 1) * block).Take(block).ToList());
            result.success = 200;
            result.message = ("🔥 lista toolCategories ottenuta con successo");
            return result;
        }

        /// <summary>
        /// [TC02] Ritorna tutti i toolCategories associati all'utente
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="index"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ToolCategoryDTO_Response> GetAllToolCategoriesByUserAsync(int userID, int index, int block)
        {
            ToolCategoryDTO_Response result = new ToolCategoryDTO_Response();

            if (index == 0 || block == 0)
            {
                Logger.Warn("[TC02A1] index o block inserito non e' valido");
                result.success = 0;
                result.message = ("[TC02A1] 🚠🥀 index o block inserito non valido");
                return result;
            }

            var entities = await _repo.GetToolCategoriesByUserAsync(userID, index, block);

            if (entities.Count == 0)
            {
                Logger.Warn("[TC02A2] nessun toolCategory trovato");
                result.success = 404;
                result.message = ("[TC02A2] 💔 nessun toolCategory trovato");
                return result;
            }

            result.success = 200;
            result.toolCategories = _mapper.Map<List<TC_DTO>>(entities);
            result.message = ("🔥 List toolcategory trovata con successo");

            return result;
        }

        /// <summary>
        /// [TC03] ritorna un toolCategory associato all'utente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ToolCategoryDTO_Response> GetByIdAsync(int id, int userID)
        {
            ToolCategoryDTO_Response result = new ToolCategoryDTO_Response();

            if (!await _repo.IsToolCategoryOwnedByUserAsync(id, userID))
            {
                Logger.Warn("[TC03A7] Il toolCategory non e' associato a questo utente");
                result.success = 404;
                result.message = ("[TC03A7] 💔 toolCategory non trovato");
                return result;
            }

            if (id == 0)
            {
                Logger.Warn("[TC03A1] L'Id inserito non e' valido");
                result.success = 0;
                result.message = ("[TC03A1] 🚠🥀 ID inserito non valido");
                return result;
            }

            var entity = await _repo.GetByIdAsync(id);

            if (entity == null)
            {
                Logger.Warn("[TC03A2] Il toolCategory non trovato");
                result.success = 404;
                result.message = ("[TC03A2] 💔 toolCategory non trovato");
                return result;
            }

            result.toolCategory_DTO = _mapper.Map<TC_DTO>(entity);
            result.success = 200;
            result.message = ("🔥 ToolCategory trovato con successo");

            return result;
        }

        /// <summary>
        /// [TC04] aggiunge un ToolCategory al database
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ToolCategoryDTO_Response> AddAsync(TC_DTO dto)
        {
            ToolCategoryDTO_Response result = new ToolCategoryDTO_Response();

            if (dto.Name.IsNullOrEmpty())
            {
                Logger.Warn("[TC04A3] Dati toolCategory non validi");
                result.success = 204;
                result.message = ("[TC04A3] 🚠🥀 Dati toolCatoegory non validi");
                return result;
            }

            var entity = _mapper.Map<ToolCategory>(dto);

            await _repo.AddAsync(entity);

            result.success = 200;
            result.toolCategory_DTO = dto;
            result.message = ("🔥 toolCatoegory aggiunto con successo");
            return result;
        }

        /// <summary>
        /// [TC05] aggiorna i dati del ToolCategory e li restituisce
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ToolCategoryDTO_Response> UpdateAsync(TC_DTO_Update dto)
        {
            ToolCategoryDTO_Response result = new ToolCategoryDTO_Response();

            if (dto.Name.IsNullOrEmpty())
            {
                Logger.Warn("[TC05A3] Name toolCatoegory non validi");
                result.success = 204;
                result.message = ("[TC05A3] 🚠🥀 Dati toolCatoegory non validi");
                return result;
            }

            if (dto.CategoryId == 0)
            {
                Logger.Warn("[TC05A1] Name toolCatoegory non validi");
                result.success = 204;
                result.message = ("[TC05A1] 🚠🥀 Dati toolCatoegory non validi");
                return result;
            }

            var entity = _mapper.Map<ToolCategory>(dto);
            await _repo.UpdateAsync(entity);

            result.success = 200;
            result.toolCategory_DTO = _mapper.Map<TC_DTO>(dto);
            result.message = ("🔥 toolCatoegory aggiornato con successo");
            return result;
        }

        /// <summary>
        /// [TC06] Elimina i dati del ToolCategory e li restituisce
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ToolCategoryDTO_Response> DeleteAsync(TC_DTO_Delete dto)
        {
            ToolCategoryDTO_Response result = new ToolCategoryDTO_Response();

            if (dto.CategoryId == 0)
            {
                Logger.Warn("[TC06A1] ID tool non validi");
                result.success = 204;
                result.message = ("[TC06A1] 🚠🥀 Dati tool non validi");
                return result;
            }

            var entity = _mapper.Map<ToolCategory>(dto);
            await _repo.DeleteAsync(entity);

            result.success = 200;
            result.toolCategory_DTO = _mapper.Map<TC_DTO>(dto);
            result.message = ("🔥 tool eliminato con successo");
            return result;
        }
    }
}