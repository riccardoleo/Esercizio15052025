using Esercizio20052025.DTO.Users_DTO;
using Esercizio20052025.Service.User_Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Esercizio20052025.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUser_Service _userService;

        public UserController(IUser_Service userService)
        {
            _userService = userService;
        }


        // ▀▄▀▄▀▄  CHIAMATE ADMIN 👑​  ▄▀▄▀▄▀ //

        [Authorize(Roles = "admin")]
        [HttpGet("getAll/{index}/{block}")]
        public async Task<IActionResult> GetAll(int index, int block)
        {
            UserResponseDTO response = new UserResponseDTO();

            response = await _userService.GetAllAsync(index, block);

            return response.success switch
            {
                200 => Ok(response),
                204 => NoContent(),
                404 => NotFound(response),
                _ => BadRequest(response),
            };
        }

        [Authorize(Roles = "admin")]
        [HttpGet("getSingle/{Id}")]
        public async Task<IActionResult> ShowSingle(int Id)
        {
            UserResponseDTO result = new UserResponseDTO();

            result = await _userService.GetByIdAsync(Id);

            return result.success switch
            {
                200 => Ok(result),
                204 => NoContent(),
                404 => NotFound(result),
                500 => StatusCode(500, result),
                _ => BadRequest(result),
            };
        }



        // ▀▄▀▄▀▄  CHIAMATE Utente 👍​  ▄▀▄▀▄▀ //

        [HttpPost("register")]
        public async Task<IActionResult> Create([FromBody] User_DTO dto)
        {
            UserResponseDTO result = new UserResponseDTO();

            result = await _userService.RegisterAsync(dto);

            return result.success switch
            {
                200 => Ok(result),
                204 => NoContent(),
                404 => NotFound(result),
                500 => StatusCode(500, result),
                _ => BadRequest(result),
            };
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserCredential_DTO dto)
        {
            UserResponseDTO result = new UserResponseDTO();

            result = _userService.Login(dto);

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
        public async Task<IActionResult> Update([FromBody] User_DTO dto)
        {
            UserResponseDTO result = new UserResponseDTO();

            result = await _userService.UpdateAsync(dto);
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
        public async Task<IActionResult> Delete([FromBody] UserCredential_DTO dto)
        {
            UserResponseDTO result = new UserResponseDTO();

            result = await _userService.DeleteAsync(dto);
            return result.success switch
            {
                200 => Ok(result),
                204 => NoContent(),
                404 => NotFound(result),
                _ => BadRequest(result),
            };
        }

        [Authorize]
        [HttpGet("check-role")]
        public IActionResult CheckRole()
        {
            UserResponseDTO result = new UserResponseDTO();

            var userRole = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            var userName = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            result = _userService.CheckRole(userRole, userName);

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
