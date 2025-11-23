using IDIMAdmin.Models.Admin;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace IDIMAdmin.Services.User
{
	public interface IActivityLogService
    {
        void InsertAsync(ActivityLogVm model);
        Task<object> GetAllAsync(ActivityLogPaginationSearchVm model);
        Task<List<ActivityLogVm>> GetUserByFilterAsync(ActivityLogSearchVm filter);
        Task<ActivityLogVm> GetByUserIdAsync(int Id);
    }
}