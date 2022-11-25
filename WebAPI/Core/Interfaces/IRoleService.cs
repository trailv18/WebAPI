using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Core.Entities;
using WebAPI.Core.Models;
using WebAPI.Core.Models.Role;

namespace WebAPI.Core.Interfaces
{
    public interface IRoleService
    {
        Task<Role> GetByIdAsync(int id);
        Task<Role> GetByNameAsync(string name);
        Task<Response> InsertAsync(RoleModel roleModel);
        Task<Response> UpdateAsync(RoleModel roleModel);
        Task<Response> DeleteAsync(int id);
        List<RoleModel> GetAll(string search, int? page, int? pageSize);
        Task<Response> AddRoleForUserAsync(int roleId, int userId);
    }
}
