using IDIMAdmin.Models.User;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace IDIMAdmin.Services.User
{
	public interface IRoleMenuPermissionService
    {
        Task InsertDeleteAsync(int roleId, IList<MenuGroupByVm> menuGroups);
    }
}
