using Esercizio15052025.DTO.ToolCategory_DTO;
using Esercizio15052025.Service.ToolsCategory_Service.Interfaces;
using Esercizio20052025.Service.User_Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Esercizio15052025.Controllers
{
    [Route("toolcategory")]
    public class ToolCategoryController : Controller
    {
        private readonly IToolCategory_Service _tcService;
        private readonly IUser_Service _userService;
        
        public ToolCategoryController (IToolCategory_Service tcService, IUser_Service user_Service)
        {
            _tcService = tcService;
            _userService = user_Service;
        }

        [Authorize]
        [HttpGet("getall/{index}/{block}")]
        public async Task<IActionResult> GetAll(int index, int block)
        {
            var entity = await _tcService.GetAllAsync(index, block);

            if (entity.Count == 0)
                return NoContent();

            return entity != null ? Ok(entity) : NotFound();
        }

        [Authorize]
        [HttpGet("getsingle/{Id}")]
        public async Task<IActionResult> ShowSingle(int Id, string username)
        {
            int userID = _userService.UserIDFromUserName(username);
            var entity = await _tcService.GetByIdAsync(Id, userID);
            return entity != null ? Ok(entity) : NotFound();
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] TC_DTO dto, string username)
        {
            int userID = _userService.UserIDFromUserName(username);
            dto.CreatedByUserId = userID;
            await _tcService.AddAsync(dto);

            if(string.IsNullOrWhiteSpace(dto.Name))
                return NotFound("Elemento non trovato o nullo");

            return Ok("Creato con successo");
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] TC_DTO_Update dto, string username)
        {
            int userID = _userService.UserIDFromUserName(username);
            dto.CreatedByUserId = userID;
            await _tcService.UpdateAsync(dto);
            return Ok("Aggiornato con successo");
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] TC_DTO_Delete dto)
        {
            await _tcService.DeleteAsync(dto);
            return Ok("Eliminato con successo");
        }
    }
}
