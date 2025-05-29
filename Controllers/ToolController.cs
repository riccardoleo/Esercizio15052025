using Esercizio15052025.DTO.Tool_DTO;
using Esercizio15052025.Service.Tool_Service.Interfeces;
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

        [Authorize(Roles = "admin")]
        [HttpGet("getallAdmin/{index}/{block}")]
        public async Task<IActionResult> GetAllAdmin(int index, int block, string username)
        {
            int userID = _userService.UserIDFromUserName(username);
            var entity = await _toolService.GetAllAsync(index, block, userID);

            if (entity.Count == 0)
                return NoContent();

            return Ok(entity);
        }

        //[Authorize]
        [HttpGet("getall/{index}/{block}")]
        public async Task<IActionResult> GetAll(int index, int block, string username)
        {
            int userID = _userService.UserIDFromUserName(username);
            var entity = await _toolService.GetAllToolsByUserAsync(userID, index, block);

            if (entity.Count == 0)
                return NoContent();

            return Ok(entity);
        }

        [Authorize]
        [HttpGet("getsingle/{Id}")]
        public async Task<IActionResult> ShowSingle(int Id, string username)
        {
            int userID = _userService.UserIDFromUserName(username);
            var entity = await _toolService.GetByIdAsync(Id, userID);
            return Ok(entity);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] T_DTO dto, string username)
        {
            int userID = _userService.UserIDFromUserName(username);
            dto.CreatedByUserId = userID;
            await _toolService.AddAsync(dto);
            return Ok("Creato con successo");
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] T_DTO_Update dto, string username)
        {
            int userID = _userService.UserIDFromUserName(username);
            dto.CreatedByUserId = userID;
            await _toolService.UpdateAsync(dto);
            return Ok("Aggiornato con successo");
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] T_DTO_Delete dto)
        {
            await _toolService.DeleteAsync(dto);
            return Ok("Eliminato con successo");
        }
    }
}

