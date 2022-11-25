using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Core.Entities;
using WebAPI.Core.Interfaces;
using WebAPI.Core.Models.Role;

namespace WebAPI.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("role")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var result = await _roleService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("role")]
        public async Task<IActionResult> CreateRole(RoleModel model)
        {
            var result = await _roleService.InsertAsync(model);
            return Ok(result);
        }

        [HttpPut("role")]
        public async Task<IActionResult> UpdateRole(RoleModel model)
        {
            var result = await _roleService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("role")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            var result = await _roleService.DeleteAsync(roleId);
            return Ok(result);
        }

        [HttpPost("add-role-user")]
        public async Task<IActionResult> AddRoleForUser(int roleId, int userId)
        {
            var result = await _roleService.AddRoleForUserAsync(roleId, userId);
            return Ok(result);
        }
    }
}
