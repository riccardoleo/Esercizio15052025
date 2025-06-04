using Esercizio15052025.DTO.Tool_DTO;
using Esercizio15052025.Service.Tool_Service.Interfeces;
using Esercizio20052025.DTO.Tool_DTO;
using Esercizio20052025.DTO.Users_DTO;
using Esercizio20052025.Service.User_Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Esercizio15052025.Controllers
{
    [Route("tool")]
    public class ToolController : Controller
    {
        
        private readonly ITool_Service _toolService;
        private readonly IUser_Service _userService;

        public ToolController(ITool_Service toolService, IUser_Service user_Service)
        {
            _toolService = toolService;
            _userService = user_Service;
        }

        
        
        // ▀▄▀▄▀▄  CHIAMATE ADMIN 👑  ▄▀▄▀▄▀ //
        
        [Authorize(Roles = "admin")]
        [HttpGet("getallAdmin/{index}/{block}")]
        public async Task<IActionResult> GetAllAdmin(int index, int block)
        {
            ToolDTO_Response result = new ToolDTO_Response();
            
            result = await _toolService.GetAllAsync(index, block);

            return result.success switch
            {
                200 => Ok(result),
                204 => NoContent(),
                404 => NotFound(result),
                _ => BadRequest(result),
            };
        }



        // ▀▄▀▄▀▄  CHIAMATE Utente 👍  ▄▀▄▀▄▀ //

        //[Authorize]
        [HttpGet("getall/{index}/{block}")]
        public async Task<IActionResult> GetAll(int index, int block, string username)
        {
            ToolDTO_Response result = new ToolDTO_Response();
            UserResponseDTO userResponseDTO = new UserResponseDTO();
            
            userResponseDTO = _userService.UserIDFromUserName(username);
            result = await _toolService.GetAllToolsByUserAsync((int)userResponseDTO.UserId, index, block);

            return result.success switch
            {
                200 => Ok(result),
                204 => NoContent(),
                404 => NotFound(result),
                _ => BadRequest(result),
            };
        }

        [Authorize]
        [HttpGet("getsingle/{Id}")]
        public async Task<IActionResult> ShowSingle(int Id, string username)
        {
            ToolDTO_Response result = new ToolDTO_Response();
            UserResponseDTO userResponseDTO = new UserResponseDTO();
            
            userResponseDTO = _userService.UserIDFromUserName(username);
            result = await _toolService.GetByIdAsync(Id, (int)userResponseDTO.UserId);
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
        public async Task<IActionResult> Create([FromBody] T_DTO dto, string username)
        {
            ToolDTO_Response result = new ToolDTO_Response();
            UserResponseDTO userResponseDTO = new UserResponseDTO();
            
            userResponseDTO = _userService.UserIDFromUserName(username);
            dto.CreatedByUserId = userResponseDTO.UserId;
            result =await _toolService.AddAsync(dto);
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
        public async Task<IActionResult> Update([FromBody] T_DTO_Update dto, string username)
        {
            ToolDTO_Response result = new ToolDTO_Response();
            UserResponseDTO userResponseDTO = new UserResponseDTO();
            
            userResponseDTO = _userService.UserIDFromUserName(username);
            dto.CreatedByUserId = userResponseDTO.UserId;
            result = await _toolService.UpdateAsync(dto);
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
        public async Task<IActionResult> Delete([FromBody] T_DTO_Delete dto)
        {
            ToolDTO_Response result = new ToolDTO_Response();

            result = await _toolService.DeleteAsync(dto);
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

