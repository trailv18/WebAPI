using System.Threading.Tasks;
using WebAPI.Core.Entities;
using WebAPI.Core.Models;
using WebAPI.Core.Models.User;

namespace WebAPI.Core.Interfaces
{
    public interface IUserService
    {
        Task<User> FindByEmailAsync(string email);
        Task<Response> Register(RegisterModel userModel);
        Task<Response> Login(LoginModel userModel);
    }
}
