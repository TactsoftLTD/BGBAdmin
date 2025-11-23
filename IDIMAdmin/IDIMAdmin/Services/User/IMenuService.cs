using DataTables.Mvc;
using IDIMAdmin.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDIMAdmin.Services.User
{
    public interface IMenuService
    {
        Task<List<MenuVm>> GetAllAsync(int? applicationId = null);
        Task<MenuVm> GetByIdAsync(int id);
        Task<MenuVm> InsertAsync(MenuVm model);
        Task InsertAllAsync(MenuGenerateVm model);
        Task<MenuVm> UpdateAsync(MenuVm model);
        Task<MenuVm> DeleteAsync(int id);
        Task InsertDeleteAsync(MenuGenerateVm model);
        Task<DataTablesResponse> GetByAsync(IDataTablesRequest requestModel, MenuSearchVm filter);
        Task<IEnumerable<SelectListItem>> GetDropDownAsync(int? selected = 0, int? applicationId = null);
        List<MenuVm> AdminMenuData();
        Task<IList<MenuGroupByVm>> GetMenuByApplication(int? appid, int? roleId);
        Task<IList<MenuGroupByVm>> GetByUserIdAsync(int userId);
    }
}