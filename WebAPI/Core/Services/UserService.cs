using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Core.Constants;
using WebAPI.Core.Data;
using WebAPI.Core.Entities;
using WebAPI.Core.Interfaces;
using WebAPI.Core.Models;
using WebAPI.Core.Models.Role;
using WebAPI.Core.Models.User;
using WebAPI.Core.Utilities;

namespace WebAPI.Core.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;
        private readonly IRoleService _roleService;
        private readonly IConfiguration _configuration;

        public UserService(ApplicationDbContext db, IRoleService roleService, IConfiguration configuration)
        {
            _db = db;
            _roleService = roleService;
            _configuration = configuration;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Response> Login(LoginModel userModel)
        {
            var user = await FindByEmailAsync(userModel.Email);

            if (user != null)
            {
                var hashCode = user.PasswordSalt;
                var encodingPasswordString = PasswordUtilities.EncodePassword(userModel.Password, hashCode);
                var userLogin = await _db.Users.FirstOrDefaultAsync(x => x.Email == userModel.Email && x.Password.Equals(encodingPasswordString));

                var roleIds = _db.UserRoles.Where(x => x.UserId == userLogin.Id).Select(x => x.RoleId);
                var roles = _db.Roles.Include(x => x.UserRoles).Where(x => roleIds.Contains(x.Id)).Select(x => x.Name);

                var authClaims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("FullName", user.FullName),
                    new Claim("Email", user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                };

                foreach (var userRole in roles)
                {
                    authClaims.Add(new Claim("Role", userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return new Response
                {
                    Status = "Success",
                    Message = "Đăng nhập thành công",
                    Data = new JwtSecurityTokenHandler().WriteToken(token)
                };
            }

            throw new AppException("Email hoặc mật khẩu không đúng!", StatusCodes.Status404NotFound);
        }

        public async Task<Response> Register(RegisterModel userModel)
        {
            var emailExists = await FindByEmailAsync(userModel.Email);
            if (emailExists != null)
                throw new AppException("Email đã tồn tại!", StatusCodes.Status404NotFound);

            var saltKey = PasswordUtilities.GeneratePassword(20);

            var user = new User
            {
                FullName = userModel.FullName,
                Email = userModel.Email,
                IsActive = true,
                Password = PasswordUtilities.EncodePassword(userModel.Password, saltKey),
                PasswordSalt = saltKey,
                CreateDate = DateTime.Now
            };
            await _db.Users.AddAsync(user);

            if (await _roleService.GetByNameAsync(RoleConstants.ROLE_ADMIN) == null)
            {
                await _roleService.InsertAsync(new RoleModel() { Name = RoleConstants.ROLE_ADMIN });
            }
            if (await _roleService.GetByNameAsync(RoleConstants.ROLE_USER) == null)
            {
                await _roleService.InsertAsync(new RoleModel() { Name = RoleConstants.ROLE_USER });

            }

            var userRole = new UserRole();
            if (string.IsNullOrWhiteSpace(userModel.Role))
            {
                var role = await _roleService.GetByNameAsync(RoleConstants.ROLE_USER);
                userRole = new UserRole
                {
                    User = user,
                    Role = role
                };
            }
            if (userModel.Role == RoleConstants.ROLE_ADMIN)
            {
                var role = await _roleService.GetByNameAsync(RoleConstants.ROLE_ADMIN);
                userRole = new UserRole
                {
                    User = user,
                    Role = role
                };
            }

            await _db.UserRoles.AddAsync(userRole);

            await _db.SaveChangesAsync();
            return new Response
            {
                Status = "Success",
                Message = "Đăng ký thành công!"
            };
        }
    }
}
