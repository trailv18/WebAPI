using System;

namespace WebAPI.Core.Models.User
{
    public class UserModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public int Coin { get; set; }
        public bool IsActive { get; set; }
        public string Mobile { get; set; }
        public string Role { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
