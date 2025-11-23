using System.Collections.Generic;

namespace IDIMAdmin.Models.User
{
	public class RoleGroupByVm
    {
        public RoleGroupByVm()
        {
            Roles = new List<RoleAssignVm>();
        }
        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public IList<RoleAssignVm> Roles { get; set; }
    }
}