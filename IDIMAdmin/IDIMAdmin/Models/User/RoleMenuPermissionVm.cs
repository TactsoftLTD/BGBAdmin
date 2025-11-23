using System;
using System.Collections.Generic;

namespace IDIMAdmin.Models.User
{
	public class RoleMenuPermissionVm
    {
        public RoleMenuPermissionVm()
        {
            Menus = new List<MenuGroupByVm>();
        }
        public int? UserId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? UpdateNo { get; set; }
        public int? ApplicationId { get; set; }
        public string ApplicationName { get; set; } 
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public IList<MenuGroupByVm> Menus { get; set; }
    }
}
