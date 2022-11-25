using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Core.Data;
using WebAPI.Core.Entities;
using WebAPI.Core.Interfaces;
using WebAPI.Core.Models;
using WebAPI.Core.Models.Role;
using WebAPI.Core.Utilities;

namespace NovelWebApp.Services
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _db;

        public RoleService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Response> InsertAsync(RoleModel roleModel)
        {
            var role = new Role
            {
                Name = roleModel.Name,
                CreateDate = DateTime.Now
            };

            _db.Roles.Add(role);
            await _db.SaveChangesAsync();
            return new Response
            {
                Status = "Success",
                Message = "Tạo mới Role thành công!"
            };
        }

        public async Task<Response> DeleteAsync(int id)
        {
            var role = await _db.Set<Role>().FirstOrDefaultAsync(x => x.Id == id);

            if (role == null)
            {
                throw new AppException("Không tìm thấy Role!", StatusCodes.Status404NotFound);
            }

            _db.Set<Role>().Remove(role);
            await _db.SaveChangesAsync();

            return new Response
            {
                Status = "Success",
                Message = "Xóa Role thành công!"
            };
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            return await _db.Set<Role>().FirstOrDefaultAsync(i => i.Id == id);
        }

        public List<RoleModel> GetAll(string search, int? page, int? pageSize)
        {
            var roles = _db.Roles.Select(x => new RoleModel
                                       {
                                           Id = x.Id,
                                           Name = x.Name,
                                           CreateDate = x.CreateDate,
                                           UpdateDate = x.UpdateDate
                                       });

            if (!string.IsNullOrEmpty(search))
            {
                roles = roles.Where(b => b.Name.ToUpper().Contains(search.ToUpper()));
            }

            return roles.OrderByDescending(x => x.CreateDate).ToList();
        }

        public async Task<Role> GetByNameAsync(string name)
        {
            return await _db.Roles.FirstOrDefaultAsync(i => name == i.Name);
        }

        public async Task<Response> UpdateAsync(RoleModel roleModel)
        {
            var role = await _db.Set<Role>().FirstOrDefaultAsync(x => x.Id == roleModel.Id);

            if (role == null)
            {
                throw new AppException("Không tìm thấy Role!", StatusCodes.Status404NotFound);
            }

            role.Name = roleModel.Name;
            role.UpdateDate = DateTime.Now;

            await _db.SaveChangesAsync();
            return new Response
            {
                Status = "Success",
                Message = "Cập nhật Role thành công!"
            };
        }

        public async Task<Response> AddRoleForUserAsync(int roleId, int userId)
        {
            var role = await GetByIdAsync(roleId);
            var user = await _db.Set<User>().FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null || role == null)
            {
                throw new AppException("Role hoặc User không tồn tại!", StatusCodes.Status404NotFound);
            }

            var addUserRole = new UserRole
            {
                RoleId = roleId,
                UserId = userId
            };
            await _db.AddAsync(addUserRole);
            await _db.SaveChangesAsync();
            return new Response
            {
                Status = "Success",
                Message = "Thêm Role cho User thành công!"
            };
        }
    }
}