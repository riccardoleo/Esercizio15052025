﻿using AutoMapper;
using Esercizio15052025.DTO.Tool_DTO;
using Esercizio15052025.Models;
using Esercizio15052025.Repository.Tool_Repo.Interfaces;
using Esercizio15052025.Service.Check_Service;
using Esercizio15052025.Service.Tool_Service.Interfeces;
using Esercizio20052025.DTO.Tool_DTO;
using Esercizio20052025.Repository.LVisibility_Repo.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Esercizio15052025.Service.Tool_Service
{
    public class Tool_Service(ITool_Repo repo, IMapper mapper, ILVisibility_Repo visibility_Repo) : ITool_Service
    {
        private readonly ITool_Repo _repo = repo;
        private readonly IMapper _mapper = mapper;
        private readonly ILVisibility_Repo _visibility_Repo = visibility_Repo;


        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// [TS01] Questo servizio ritorna un oggetto contenente una List Tool_DTO con tutti gli tool esistenti
        /// </summary>
        /// <param name="index"></param>
        /// <param name="block"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<ToolDTO_Response> GetAllAsync(int index, int block)
        {
            ToolDTO_Response result = new ToolDTO_Response();
            
            if (index == 0 || block == 0)
            {
                Logger.Error("[TS01A1] 0 non e' un numero valido");
                result.success = 0;
                result.message = ("[TS01A1] 🚠🥀 0 non e' un numero valido");
                return result;
            }

            List<Tool> entity = await _repo.GetAllAsync();

            if(entity.Count == 0)
            {
                Logger.Error("[TS01A2] nessun Tool trovato");
                result.success = 404;
                result.message = ("[TS01A2] 💔 nessun Tool trovato");
                return result;
            }
            
            result.tools = _mapper.Map<List<T_DTO>>(entity.Skip((index - 1) * block).Take(block).ToList());
            result.success = 200;
            result.message = ("🔥 lista tools ottenuta con successo");

            return result;
        }

        /// <summary>
        /// [TS02] Ritorna tutti i tools associati all'utente
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="index"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ToolDTO_Response> GetAllToolsByUserAsync(int userID, int index, int block)
        {
            ToolDTO_Response result = new ToolDTO_Response();

            if (index == 0 || block == 0)
            {
                Logger.Warn("[TS02A1] index o block inserito non e' valido");
                result.success = 0;
                result.message = ("[TS02A1] 🚠🥀 index o block inserito non valido");
                return result;
            }

            List<int> permissionID = await _visibility_Repo.GetPermissionIdsByUserIdAsync(userID);

            var entities = await _repo.GetAllToolsByUserAsync(userID, index, block, permissionID);

            if (entities == null)
            {
                Logger.Warn("[TS02A3] L'Id inserito non e' valido");
                result.success = 204;
                result.message = ("[TS02A3] 🚠🥀 L'Id inserito non e' valido");
                return result;
            }
            
            result.success = 200;
            result.tools = _mapper.Map<List<T_DTO>>(entities);
            result.message = ("🔥 List tool trovata con successo");

            return result;
        }
        
        /// <summary>
        /// [TS03] ritorna un Tool associato all'utente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ToolDTO_Response?> GetByIdAsync(int id, int userID)
        {
            ToolDTO_Response result = new ToolDTO_Response();

            if (!await _repo.IsToolOwnedByUserAsync(id, userID))
            {
                Logger.Warn("[TS03A7] Il tool non e' associato a questo utente");
                result.success = 404;
                result.message = ("[TS03A7] 💔 tool non trovato");
                return result;            
            }

            if (id == 0)
            {
                Logger.Warn("[TS03A1] L'Id inserito non e' valido");
                result.success = 0;
                result.message = ("[TS03A1] 🚠🥀 ID inserito non valido");
                return result;
            }
            
            var entity = await _repo.GetByIdAsync(id);

            if (entity == null)
            {
                Logger.Warn("[TS03A2] Il tool non e' stato trovato nel db");
                result.success = 404;
                result.message = ("[TS03A2] 💔 tool non trovato");
                return result;            
            }

            result.tool_DTO = _mapper.Map<T_DTO>(entity);
            result.success = 200;
            result.message = ("🔥 Tool trovato con successo");

            return result;
        }

        /// <summary>
        /// [TS04] Aggiunge un Tool al database
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ToolDTO_Response> AddAsync(T_DTO dto)
        {
            ToolDTO_Response result = new ToolDTO_Response();

            if (dto.Name.IsNullOrEmpty())
            {
                Logger.Warn("[TS04A3] Dati tool non validi");
                result.success = 204;
                result.message = ("[TS04A3] 🚠🥀 Dati tool non validi");
                return result;
            }
            
            if (_repo.ExistsByName(dto.Name))
            {
                Logger.Warn("[TS04A5] Il nome e' gia' esistente");
                result.success = 0;
                result.message = ("[TS04A5] 🚠🥀 name inserito gia' esistente");
                return result;            
            }

            var entity = _mapper.Map<Tool>(dto);
            
            if (dto.CreationDate == DateTime.MinValue)
                dto.CreationDate = DateTime.Now;
            
            await _repo.AddAsync(entity);
            
            result.success = 200;
            result.tool_DTO = dto;
            result.message = ("🔥 Tool aggiunto con successo");
            return result;
        }

        /// <summary>
        /// [TS05] Aggiorna i dati del Tool e li restituisce
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ToolDTO_Response> UpdateAsync(T_DTO_Update dto)
        {
            ToolDTO_Response result = new ToolDTO_Response();

            var Tool = await _repo.GetByIdAsync(dto.ToolId);
            
            if (Tool.Name != dto.Name && _repo.ExistsByName(dto.Name))
            {
                Logger.Warn("[TS05A5] Il nome e' gia' esistente");
                result.success = 0;
                result.message = ("[TS05A5] 🚠🥀 name inserito gia' esistente");
                return result;            
            }
            
            if (dto.Name.IsNullOrEmpty())
            {
                Logger.Warn("[TS05A3] Name tool non validi");
                result.success = 204;
                result.message = ("[TS05A3] 🚠🥀 Dati tool non validi");
                return result;
            }
            
            if (dto.ToolId == 0)
            {
                Logger.Warn("[TS05A1] Name tool non validi");
                result.success = 204;
                result.message = ("[TS05A1] 🚠🥀 Dati tool non validi");
                return result;
            }
            
            var entity = _mapper.Map<Tool>(dto);
            await _repo.UpdateAsync(entity);
            
            result.success = 200;
            result.tool_DTO = _mapper.Map<T_DTO>(entity);
            result.message = ("🔥 Tool aggiornato con successo");
            return result;
        }

        /// <summary>
        /// [TS06] Elimina i dati del Tool e li restituisce
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ToolDTO_Response> DeleteAsync(T_DTO_Delete dto)
        {
            ToolDTO_Response result = new ToolDTO_Response();

            if (dto.ToolId == 0)
            {
                Logger.Warn("[TS06A1] ID tool non validi");
                result.success = 204;
                result.message = ("[TS06A1] 🚠🥀 Dati tool non validi");
                return result;
            }

            var entity = _mapper.Map<Tool>(dto);
            await _repo.DeleteAsync(entity);

            result.success = 200;
            result.tool_DTO = _mapper.Map<T_DTO>(entity);
            result.message = ("🔥 tool eliminato con successo");
            return result;
        }


    }
}
