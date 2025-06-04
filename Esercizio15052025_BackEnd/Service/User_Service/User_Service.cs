using AutoMapper;
using Esercizio15052025.Models;
using Esercizio15052025.Service.Check_Service;
using Esercizio20052025.DTO.Users_DTO;
using Esercizio20052025.Repository.User_Repo.Interfaces;
using Esercizio20052025.Service.User_Service.Interfaces;
using Microsoft.Extensions.ObjectPool;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Esercizio20052025.Service.User_Service
{
    public class User_Service(IUser_Repo repo, IMapper mapper) : IUser_Service
    {
        private readonly IUser_Repo _repo = repo;
        private readonly IMapper _mapper = mapper;

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// [US01] Questo servizio ritorna un oggetto contenente una List User_DTO con tutti gli user esistenti
        /// </summary>
        /// <param name="index"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        public async Task<UserResponseDTO> GetAllAsync(int index, int block)
        {
            UserResponseDTO result = new UserResponseDTO();

            if (index == 0 || block == 0)
            {
                Logger.Error("[US01A1] 0 non e' un numero valido");
                result.success = 0;
                result.message = ("[US01A1] 🚠🥀 0 non e' un numero valido");
                return result;
            }

            List<User> entity = await _repo.GetAllAsync();

            if(entity.Count == 0)
            {
                Logger.Error("[US01A2] nessun utente trovato");
                result.success = 404;
                result.message = ("[US01A2] 💔 nessun utente trovato");
                return result;
            }

            if (entity.Count == 1)
            {
                result.success = 200;
                result.message = ("💔 sei solo amico");
                return result;
            }

            result.users = _mapper.Map <List<User_DTO>>(entity.Skip((index - 1) * block).Take(block).ToList());
            result.success = 200;
            result.message = ("🔥 lista utenti ottenuta con successo");

            return result;
        }

        /// <summary>
        /// [US02] Questo servizio ritorna un oggetto contenente un User_DTO con l'ID associato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserResponseDTO> GetByIdAsync(int id)
        {
            UserResponseDTO result = new UserResponseDTO();

            var entity = await _repo.GetByIdAsync(id);

            if (entity == null)
            {
                Logger.Error("[US02A3] L'Id inserito non e' valido");
                result.success = 204;
                result.message = ("[US0A3] 🚠🥀 L'Id inserito non e' valido");
                return result;
            }

            result.success = 200;
            result.user_DTO = _mapper.Map<User_DTO>(entity);
            result.message = ("🔥 Utente trovato con successo");

            return result;
        }

        /// <summary>
        /// [US03] Questo servizio controlla le credenziali dell'utente e resituisce il token JWT
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public UserResponseDTO Login(UserCredential_DTO dto)
        {
            UserResponseDTO result = new UserResponseDTO();

            if (dto == null)
            {
                Logger.Warn("[US03A3] Dati utente non validi");
                result.success = 204;
                result.message = ("[US03A3] 🚠🥀 Dati utente non validi");
                return result;
            }

            if (!_repo.ExistsByNameBool(dto.Username))
            {
                Logger.Warn("[US03A2] Utente non trovato");
                result.success = 404;
                result.message = ("[US03A2] 💔 Utente non trovato");
                return result;
            }

            var userInDb = _repo.ReturnUserByName(dto.Username);

            if (!BCrypt.Net.BCrypt.Verify(dto.PasswordHash, userInDb.PasswordHash))
            {
                Logger.Warn("[US03A4] La password è errata");
                result.success = 0;
                result.message = ("[US03A4] 🚠🥀 Dati utente non validi");
                return result;
            }


            var claims = new[]
            {
                new Claim(ClaimTypes.Name, dto.Username),
                new Claim(ClaimTypes.Role, userInDb.Role)
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("pippo1234pippo1234pippo1234pippo1234"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:7121",
                audience: "https://localhost:7121",
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            result.UserRole = userInDb.Role;
            result.success = 200;
            result.message = ("🔥 Utente loggato con successo");
            result.token = tokenString;

            return result;
        }

        /// <summary>
        /// [US04] Questo servizio registra l'utente e restituisce solo la risposta http e il messaggio
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<UserResponseDTO> RegisterAsync(User_DTO dto)
        {
            UserResponseDTO result = new UserResponseDTO();

            if (dto == null)
            {
                Logger.Warn("[US04A3] Dati utente non validi");
                result.success = 204;
                result.message = ("[US04A3] 🚠🥀 Dati utente non validi");
                return result;
            }

            if (_repo.ExistsByNameBool(dto.Username))
            {
                Logger.Warn("[US04A5] Il nome è già esistente");
                result.success = 0;
                result.message = ("[US04A5] 🚠🥀 Il nome è già esistente");
                return result;
            }

            if (dto.passwdRole == "adminPassword")
                dto.Role = "admin";
            else
                dto.Role="user";

            dto.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);

            if (dto.Username.IsNullOrEmpty())
            {
                Logger.Warn("[US04A3] Dati utente non validi");
                result.success = 204;
                result.message = ("[US04A3] 🚠🥀 Dati utente non validi");
                return result;
            }

            var entity = _mapper.Map<User>(dto);

            try
            {
                await _repo.AddAsync(entity);
                result.success = 200;
                result.message = ("🔥 Utente loggato con successo");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Qualcosa è andato storto: {}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// [US05] Aggiorna i dati dell'utente e li restituisce
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<UserResponseDTO> UpdateAsync(User_DTO dto)
        {
            UserResponseDTO result = new UserResponseDTO();

            if (dto.Username.IsNullOrEmpty())
            {
                Logger.Warn("[US05A3] Dati utente non validi");
                result.success = 204;
                result.message = ("[US05A3] 🚠🥀 Dati utente non validi");
                return result;
            }

            var userInDb = _repo.ReturnUserByName(dto.Username);
            if (!BCrypt.Net.BCrypt.Verify(dto.PasswordHash, userInDb.PasswordHash))
            {
                Logger.Warn("[US05A4] La password è errata");
                result.success = 0;
                result.message = ("[US05A4] 🚠🥀 Dati utente non validi");
                return result;
            }

            var entity = _mapper.Map<User>(dto);
            await _repo.UpdateAsync(entity);

            result.user_DTO = dto;
            result.user_DTO.PasswordHash = ("##############");
            result.success = 200;
            result.message = ("🔥 Utente aggiornato con successo");
            return result;
        }


        /// <summary>
        /// [US06] Elimina l'utente dal database
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<UserResponseDTO> DeleteAsync(UserCredential_DTO dto)
        {
            UserResponseDTO result = new UserResponseDTO();

            if (dto.Username == null)
            {
                Logger.Warn("[US06A3] Dati utente non validi");
                result.success = 204;
                result.message = ("[US06A3] 🚠🥀 Dati utente non validi");
                return result;
            }

            var userInDb = _repo.ReturnUserByName(dto.Username);
            if (!BCrypt.Net.BCrypt.Verify(dto.PasswordHash, userInDb.PasswordHash))
            {
                Logger.Warn("[US06A4] La password è errata");
                result.success = 0;
                result.message = ("[US06A4] 🚠🥀 Dati utente non validi");
                return result;
            }

            var existUser = _repo.ReturnUserDtoByName(dto.Username);
            var entity = _mapper.Map<User>(existUser);
            await _repo.DeleteAsync(entity);

            result.success = 200;
            result.message = ("🔥 Utente eliminato con successo");
            return result;
        }

        /// <summary>
        /// [US07] usando l'username dell'utente restituisce l'ID
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserResponseDTO UserIDFromUserName(string username)
        {
            UserResponseDTO result = new UserResponseDTO();

            if (username == null)
            {
                Logger.Warn("[US07A4] Dati utente non validi");
                result.success = 204;
                result.message = ("[US07A4] 🚠🥀 Dati utente non validi");
                return result;
            }

            if (!_repo.ExistsByNameBool(username))
            {
                Logger.Warn("[US07A2] Utente non trovato");
                result.success = 404;
                result.message = ("[US07A2] 💔 Utente non trovato");
                return result;
            }

            var userInDb = _repo.ReturnUserByName(username);

            result.UserId = userInDb.UserId;
            result.success = 200;
            result.message = ("🔥 ID dell'utente trovato con successo");
            return result;

        }

        /// <summary>
        /// [US08] Controlla che ruolo ha l'utente
        /// </summary>
        /// <param name="userRole"></param>
        /// <returns></returns>
        public UserResponseDTO CheckRole(String userRole, String userName)
        {
            UserResponseDTO result = new UserResponseDTO();
            User_DTO userDTO = new User_DTO();
            var userInDb = _repo.ReturnUserByName(userName);

            userRole = userRole.ToLower();

            if(userRole == null)
            {
                Logger.Warn("[US08A3] Dati utente non validi");
                result.success = 204;
                result.message = ("[US08A3] 🚠🥀 Dati utente non validi");
                return result;
            }

            if(userRole.ToLower() == "admin")
            {
                result.UserRole = "admin";
                result.UserName = userInDb.Username;
                result.success = 200;
                result.message = ("🔥 L'utente e' admin");
                return result;
                
            }else if(userRole.ToLower() == "user")
            {
                result.UserRole = "user";
                result.UserName = userInDb.Username;
                result.success = 200;
                result.message = ("🔥 L'utente e' user");
                return result;
            }

            Logger.Error("[US08A6] il ruolo ricevuto non e' accettabile");
            result.success = 0;
            result.message = ("🚠🥀 Ruolo utente non valido");
            return result;
        }
    }
}
