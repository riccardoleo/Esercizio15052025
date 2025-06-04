using AutoMapper;
using Esercizio15052025.DTO.PlantComponent_DTO;
using Esercizio15052025.DTO.Tool_DTO;
using Esercizio15052025.Models;
using Esercizio15052025.Repository.PlantComponent_Repo.Interfaces;
using Esercizio15052025.Service.Check_Service;
using Esercizio15052025.Service.PlantComponent_Service.Interfaces;
using Esercizio20052025.DTO.PlantComponent_DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esercizio15052025.Service.PlantComponent_Service
{
    public class PlantComponentService(IPlantComponent_Repo repo, IMapper mapper) : IPlantComponentService
    {
        private readonly IPlantComponent_Repo _repo = repo;
        private readonly IMapper _mapper = mapper;

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// [PC01] Ritorna la lista di tutti i plant component esistenti
        /// </summary>
        /// <param name="index"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        public async Task<PlantComponent_Response> GetAllAsync(int index, int block)
        {
            PlantComponent_Response result = new PlantComponent_Response();

            if (index == 0 || block == 0)
            {
                Logger.Warn("[PC01A1] 0 non e' un numero valido");
                result.success = 0;
                result.message = ("[PC01A1] 🚠🥀 0 non e' un numero valido");
                return result;
            }

            var entity = await _repo.GetAllAsync();

            if (entity.Count == 0)
            {
                Logger.Warn("[PC01A2] nessun utente trovato");
                result.success = 404;
                result.message = ("[PC01A2] 💔 nessun utente trovato");
                return result;
            }

            result.List_PC_DTO = _mapper.Map<List<PC_DTO>>(entity.Skip((index - 1) * block).Take(block).ToList());
            result.success = 200;
            result.message = ("🔥 lista PlantComponent ottenuta con successo");

            return result;
        }

        /// <summary>
        /// [PC02] Ritorna tutti i plant component associati all'utente
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="index"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        public async Task<PlantComponent_Response> GetAllPlantComponentsByUserAsync(int userID, int index, int block)
        {
            PlantComponent_Response result = new PlantComponent_Response();

            var entities = await _repo.GetPlantComponentsByUserAsync(userID, index, block);

            if (entities == null)
            {
                Logger.Warn("[PC02A3] L'Id inserito non e' valido");
                result.success = 204;
                result.message = ("[PC02A3] 🚠🥀 L'Id inserito non e' valido");
                return result;
            }

            result.success = 200;
            result.List_PC_DTO = _mapper.Map<List<PC_DTO>>(entities);
            result.message = ("🔥 List Plant Component trovata con successo");

            return result;
        }

        /// <summary>
        /// [PC03] ritorna un plant component associato all'utente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<PlantComponent_Response> GetByIdAsync(int id, int userID)
        {
            PlantComponent_Response result = new PlantComponent_Response();

            if (!await _repo.IsPlantComponentOwnedByUserAsync(id, userID))
            {
                Logger.Warn("[PC03A2] Il tool non e' associato a questo utente");
                result.success = 404;
                result.message = ("[PC03A2] 💔 plant component non trovato");
                return result;
            }

            if (id == 0)
            {
                Logger.Warn("[PC03A1] L'Id inserito non e' valido");
                result.success = 0;
                result.message = ("[PC03A1] 🚠🥀 ID inserito non valido");
                return result;
            }

            var entity = await _repo.GetByIdAsync(id);

            if (entity == null)
            {
                Logger.Warn("[PC03A2] Il plant component non e' associato a questo utente");
                result.success = 404;
                result.message = ("[PC03A2] 💔 plant component non trovato");
                return result;
            }

            result.PC_DTO = _mapper.Map<PC_DTO>(entity);
            result.success = 200;
            result.message = ("🔥 Plant Component trovato con successo");

            return result;
        }

        /// <summary>
        /// [PC04] aggiunge un Plant Component al database
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<PlantComponent_Response> AddAsync(PC_DTO dto)
        {
            PlantComponent_Response result = new PlantComponent_Response();

            if (dto.Name.IsNullOrEmpty())
            {
                Logger.Warn("[PC04A3] Dati plant component non validi");
                result.success = 204;
                result.message = ("[PC04A3] 🚠🥀 Dati plant component non validi");
                return result;
            } 

            var entity = _mapper.Map<PlantComponent>(dto);

            await _repo.AddAsync(entity);

            result.success = 200;
            result.PC_DTO = dto;
            result.message = ("🔥 Plant Component aggiunto con successo");
            return result;
        }

        /// <summary>
        /// [PC05] Aggiorna i dati del plant component e li restituisce
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<PlantComponent_Response> UpdateAsync(PC_DTO_Update dto)
        {
            PlantComponent_Response result = new PlantComponent_Response();

            if (dto.Name.IsNullOrEmpty())
            {
                Logger.Warn("[PC05A3] Name plant component non validi");
                result.success = 204;
                result.message = ("[PC05A3] 🚠🥀 Dati plant component non validi");
                return result;
            }

            if(dto.ComponentId == 0)
            {
                Logger.Warn("[PC05A1] ID plant component non validi");
                result.success = 204;
                result.message = ("[PC05A1] 🚠🥀 Dati plant component non validi");
                return result;
            }

            var entity = _mapper.Map<PlantComponent>(dto);
            await _repo.UpdateAsync(entity);

            result.success = 200;
            result.PC_DTO = _mapper.Map<PC_DTO>(entity);
            result.message = ("🔥 Plant Component aggiornato con successo");
            return result;
        }

        /// <summary>
        /// [PC06] Elimina i dati del plant component e li restituisce 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<PlantComponent_Response> DeleteAsync(PC_DTO_Delete dto)
        {
            PlantComponent_Response result = new PlantComponent_Response();

            if (dto.componentId == 0)
            {
                Logger.Warn("[PC06A1] ID plant component non validi");
                result.success = 204;
                result.message = ("[PC06A1] 🚠🥀 Dati plant component non validi");
                return result;
            }

            var entity = _mapper.Map<PlantComponent>(dto);
            await _repo.DeleteAsync(entity);

            result.success = 200;
            result.PC_DTO = _mapper.Map<PC_DTO>(entity);
            result.message = ("🔥 Plant Component eliminato con successo");
            return result;
        }
    }
}
