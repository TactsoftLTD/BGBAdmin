using System;
using System.Collections.Generic;

namespace IDIMAdmin.Models.User
{
	public class UserRolePermissionVm
    {
        public UserRolePermissionVm()
        {
            Roles = new List<RoleGroupByVm>();
        }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdateNo { get; set; }
        public IList<RoleGroupByVm> Roles { get; set; }
    }
}