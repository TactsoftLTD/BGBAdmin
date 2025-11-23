using DataTables.Mvc;
using IDIMAdmin.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IDIMAdmin.Services.User
{
    public interface IRoleService
    {
        Task<RoleVm> GetByIdAsync(int id);
        Task<RoleVm> InsertAsync(RoleVm model);
        Task<RoleVm> UpdateAsync(RoleVm model);
        Task<RoleVm> DeleteAsync(int id);
        Task<DataTablesResponse> GetByAsync(IDataTablesRequest requestModel, RoleSearchVm filter);
        Task<IList<RoleGroupByVm>> GetByUserIdAsync(int userId);
        Task<RoleVm> GetRoleInfo(int? id, bool checkActive = false);
    }
}
