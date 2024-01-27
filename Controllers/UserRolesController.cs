using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ApelMusicAPI.Models;
using ApelMusicAPI.DTOs;
using ApelMusicAPI.Data;

namespace ApelMusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly UserRolesData userRolesData;

        public UserRolesController(UserRolesData userRolesData)
        {
            this.userRolesData = userRolesData;
        }

        [HttpGet("GetRoles")]
        public IActionResult GetRoles()
        {
            try
            {
                List<UserRoles> roles = userRolesData.GetAll();
                return StatusCode(200, roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetRole")]
        public IActionResult GetRoleById(int role_id)
        {
            UserRoles? role = userRolesData.GetById(role_id);

            if (role == null) return StatusCode(404, "Data Not Found");

            return StatusCode(200, role);
        }

        [HttpPost("AddRole")]
        public IActionResult InsertNewRole([FromBody] UserRolesDTO userRolesDTO)
        {
            if (userRolesDTO == null) return BadRequest("Data should be inputed");

            UserRoles newRole = new UserRoles
            {
                roleName = userRolesDTO.roleName
            };

            bool result = userRolesData.InsertNewRole(newRole);

            if (result) return StatusCode(200);
            else return StatusCode(500, "Error Occur");
        }

        [HttpPut("UpdateRole")]
        public IActionResult UpdateRole(int role_id, [FromBody] UserRolesDTO userRolesDTO)
        {
            if (userRolesDTO == null) return BadRequest("Data should be inputed");

            UserRoles newRole = new UserRoles
            {
                roleName = userRolesDTO.roleName
            };

            bool result = userRolesData.UpdateRole(role_id, newRole);

            if (result) return StatusCode(200);
            else return StatusCode(500, "Error Occur");
        }

        [HttpDelete("DeleteRole")]
        public IActionResult DeleteRole(int role_id)
        {
            bool result = userRolesData.DeleteById(role_id);

            if (result) return StatusCode(200);
            else return StatusCode(500, "Error Occur");
        }

    }
}
