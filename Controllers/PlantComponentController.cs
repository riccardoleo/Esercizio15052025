using Esercizio15052025.DTO.PlantComponent_DTO;
using Esercizio15052025.Service.PlantComponent_Service.Interfaces;
using Esercizio20052025.DTO.PlantComponent_DTO;
using Esercizio20052025.DTO.Users_DTO;
using Esercizio20052025.Service.User_Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Esercizio15052025.Controllers
{
    // [Route("plantComponent/[controller]")]

    [Route("plantComponent")]
    public class PlantComponentController(IPlantComponentService pcService, IUser_Service user_Service) : Controller
    {

        private readonly IPlantComponentService _pcService = pcService;
        private readonly IUser_Service _userService = user_Service;



        // ▀▄▀▄▀▄  CHIAMATE ADMIN 👑​  ▄▀▄▀▄▀ //

        [Authorize(Roles = "admin")]
        [HttpGet("getAllAdmin/{index}/{block}")]
        public async Task<IActionResult> GetAll(int index, int block)
        {
            PlantComponent_Response result = new PlantComponent_Response();

            result = await _pcService.GetAllAsync(index, block);

            return result.success switch
            {
                200 => Ok(result),
                204 => NoContent(),
                404 => NotFound(result),
                _ => BadRequest(result),
            };
        }



        // ▀▄▀▄▀▄  CHIAMATE Utente 👍​  ▄▀▄▀▄▀ //

        [Authorize]
        [HttpGet("getall/{index}/{block}")]
        public async Task<IActionResult> GetAll(int index, int block, string username)
        {
            PlantComponent_Response result = new PlantComponent_Response();
            UserResponseDTO userResponseDTO = new UserResponseDTO();

            userResponseDTO = _userService.UserIDFromUserName(username);

            result = await _pcService.GetAllPlantComponentsByUserAsync((int)userResponseDTO.UserId, index, block);

            return result.success switch
            {
                200 => Ok(result),
                204 => NoContent(),
                404 => NotFound(result),
                _ => BadRequest(result),
            };
        }

        [Authorize]
        [HttpGet("getSingle/{Id}")]
        public async Task<IActionResult> ShowSingle(int Id, string username)
        {
            PlantComponent_Response result = new PlantComponent_Response();
            UserResponseDTO userResponseDTO = new UserResponseDTO();

            userResponseDTO = _userService.UserIDFromUserName(username);

            result = await _pcService.GetByIdAsync(Id, (int)userResponseDTO.UserId);

            return result.success switch
            {
                200 => Ok(result),
                204 => NoContent(),
                404 => NotFound(result),
                _ => BadRequest(result),
            };
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] PC_DTO dto, string username)
        {
            PlantComponent_Response result = new PlantComponent_Response();
            UserResponseDTO userResponseDTO = new UserResponseDTO();

            userResponseDTO = _userService.UserIDFromUserName(username);

            dto.CreatedByUserId = (int)userResponseDTO.UserId;

            result = await _pcService.AddAsync(dto);
            return result.success switch
            {
                200 => Ok(result),
                204 => NoContent(),
                404 => NotFound(result),
                _ => BadRequest(result),
            };
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] PC_DTO_Update dto, string username)
        {
            PlantComponent_Response result = new PlantComponent_Response();
            UserResponseDTO userResponseDTO = new UserResponseDTO();

            userResponseDTO = _userService.UserIDFromUserName(username);

            dto.CreatedByUserId = (int)userResponseDTO.UserId;

            result = await _pcService.UpdateAsync(dto);
            return result.success switch
            {
                200 => Ok(result),
                204 => NoContent(),
                404 => NotFound(result),
                _ => BadRequest(result),
            };
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] PC_DTO_Delete dto)
        {
            PlantComponent_Response result = new PlantComponent_Response();

            result = await _pcService.DeleteAsync(dto);
            return result.success switch
            {
                200 => Ok(result),
                204 => NoContent(),
                404 => NotFound(result),
                _ => BadRequest(result),
            };
        }
    }
}
