using Esercizio20052025.DTO.ListVisibility_DTO;
using Esercizio20052025.Service.LPermission_Service.Interfaces;
using Esercizio20052025.Service.LVisibility_Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Esercizio20052025.Controllers
{
    [Route("user-permission")]
    [ApiController]
    public class UserPermissionController : Controller
    {
        private readonly ILPermission_Service _permissionService;
        private readonly ILVisibility_Service _lVisibility_Service;

        public UserPermissionController(ILPermission_Service permissionService, ILVisibility_Service lVisibility_Service)
        {
            _permissionService = permissionService;
            _lVisibility_Service = lVisibility_Service;
        }

        /// Visibility API

        [Authorize(Roles = "admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            LVisibilityResponse response = new();

            response = await _lVisibility_Service.GetAllAsync();

            return response.success switch
            {
                200 => Ok(response),
                204 => NoContent(),
                404 => NotFound(response),
                _ => BadRequest(response),
            };
        }

        [HttpGet("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync(int ID)
        {
            LVisibilityResponse response = new();

            response = await _lVisibility_Service.GetByIdAsync(ID);

            return response.success switch
            {
                200 => Ok(response),
                204 => NoContent(),
                404 => NotFound(response),
                _ => BadRequest(response),
            };
        }

        [HttpPost("")]
        public async Task<IActionResult> AddAsync(int UserID, int PermissionID)
        {

        }
        public async Task<IActionResult> DeleteAsync(ListVisibility_DTO item)
        {

        }
    }
}
