using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Core.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [StringLength(512)]
        [Required]
        public string Name { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public virtual IEnumerable<UserRole> UserRoles { get; set; }

    }
}