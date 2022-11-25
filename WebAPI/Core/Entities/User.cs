using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Core.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [StringLength(512)]
        [Required]
        public string FullName { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(512)]
        [Required]
        public string Password { get; set; }

        [StringLength(512)]
        [Required]
        public string PasswordSalt { get; set; }

        public int Coin { get; set; }

        public bool IsActive { get; set; }

        [StringLength(50)]
        public string Mobile { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(1024)]
        public string Address { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public virtual IEnumerable<UserRole> UserRoles { get; set; }

    }
}