using System;

namespace WebAPI.Core.Models.Role
{
    public class RoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
