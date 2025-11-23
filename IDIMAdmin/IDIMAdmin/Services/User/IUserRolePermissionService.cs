using IDIMAdmin.Models.User;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace IDIMAdmin.Services.User
{
	public interface IUserRolePermissionService
    {
        Task InsertDeleteAsync(int userId, IList<RoleGroupByVm> roleGroups);
    }
}
