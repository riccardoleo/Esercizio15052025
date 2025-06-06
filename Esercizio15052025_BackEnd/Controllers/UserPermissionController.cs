using Esercizio20052025.DTO.ListPermission_DTO;
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
        [HttpGet("VisbilityGetAll")]
        public async Task<IActionResult> VisbilityGetAll()
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

        [HttpGet("VisbilityGetByIdAsync")]
        public async Task<IActionResult> VisbilityGetByIdAsync(int ID)
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

        [HttpPost("VisbilityAddAsync")]
        public async Task<IActionResult> VisbilityAddAsync(int UserID, int PermissionID)
        {
            LVisibilityResponse response = new();

            response = await _lVisibility_Service.AddAsync(UserID, PermissionID);

            return response.success switch
            {
                200 => Ok(response),
                204 => NoContent(),
                404 => NotFound(response),
                _ => BadRequest(response),
            };
        }

        [HttpDelete("VisbilityDeleteAsync")]
        public async Task<IActionResult> VisbilityDeleteAsync(ListVisibility_DTO item)
        {
            LVisibilityResponse response = new();

            response = await _lVisibility_Service.DeleteAsync(item);

            return response.success switch
            {
                200 => Ok(response),
                204 => NoContent(),
                404 => NotFound(response),
                _ => BadRequest(response),
            };
        }


        // Permission API

        [Authorize(Roles = "admin")]
        [HttpGet("PermissionGetAll")]
        public async Task<IActionResult> PermissionGetAll()
        {
            LPermissionResponse response = new();

            response = await _permissionService.GetAllAsync();

            return response.success switch
            {
                200 => Ok(response),
                204 => NoContent(),
                404 => NotFound(response),
                _ => BadRequest(response),
            };
        }

        [HttpGet("PermissionGetByIdAsync")]
        public async Task<IActionResult> PermissionGetByIdAsync(int ID)
        {
            LPermissionResponse response = new();

            response = await _permissionService.GetByIdAsync(ID);

            return response.success switch
            {
                200 => Ok(response),
                204 => NoContent(),
                404 => NotFound(response),
                _ => BadRequest(response),
            };
        }

        [HttpPost("PermissionAddAsync")]
        public async Task<IActionResult> PermissionAddAsync(int UserID, int PermissionID)
        {
            LPermissionResponse response = new();

            response = await _permissionService.AddAsync(UserID, PermissionID);

            return response.success switch
            {
                200 => Ok(response),
                204 => NoContent(),
                404 => NotFound(response),
                _ => BadRequest(response),
            };
        }

        [HttpDelete("PermissionDeleteAsync")]
        public async Task<IActionResult> PermissionDeleteAsync(ListPermission_DTO item)
        {
            LPermissionResponse response = new();

            response = await _permissionService.DeleteAsync(item);

            return response.success switch
            {
                200 => Ok(response),
                204 => NoContent(),
                404 => NotFound(response),
                _ => BadRequest(response),
            };
        }
    }
}
