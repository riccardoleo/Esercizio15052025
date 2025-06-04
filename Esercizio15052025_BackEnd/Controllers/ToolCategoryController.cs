using Esercizio15052025.DTO.ToolCategory_DTO;
using Esercizio15052025.Service.ToolsCategory_Service.Interfaces;
using Esercizio20052025.DTO.Users_DTO;
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
            ToolCategoryDTO_Response result = new ToolCategoryDTO_Response();
            
            result = await _tcService.GetAllAsync(index, block);

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
            ToolCategoryDTO_Response result = new ToolCategoryDTO_Response();
            UserResponseDTO userResponseDTO = new UserResponseDTO();

            userResponseDTO = _userService.UserIDFromUserName(username);
            result = await _tcService.GetByIdAsync(Id, (int)userResponseDTO.UserId);
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
        public async Task<IActionResult> Create([FromBody] TC_DTO dto, string username)
        {
            ToolCategoryDTO_Response result = new ToolCategoryDTO_Response();
            UserResponseDTO userResponseDTO = new UserResponseDTO();
            
            userResponseDTO = _userService.UserIDFromUserName(username);
            dto.CreatedByUserId = userResponseDTO.UserId;
            result = await _tcService.AddAsync(dto);
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
        public async Task<IActionResult> Update([FromBody] TC_DTO_Update dto, string username)
        {
            ToolCategoryDTO_Response result = new ToolCategoryDTO_Response();UserResponseDTO userResponseDTO = new UserResponseDTO();
            
            userResponseDTO = _userService.UserIDFromUserName(username);
            dto.CreatedByUserId = userResponseDTO.UserId;
            result = await _tcService.UpdateAsync(dto);
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
        public async Task<IActionResult> Delete([FromBody] TC_DTO_Delete dto)
        {
            ToolCategoryDTO_Response result = new ToolCategoryDTO_Response();

            result = await _tcService.DeleteAsync(dto);
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
