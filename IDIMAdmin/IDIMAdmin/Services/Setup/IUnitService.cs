using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using IDIMAdmin.Models.Setup;

namespace IDIMAdmin.Services.Setup
{
    public interface IUnitService
    {
        Task<List<UnitVm>> GetAllAsync();
        Task<UnitVm> GetByIdAsync(int id);
        Task<UnitVm> InsertAsync(UnitVm model);
        Task<UnitVm> UpdateAsync(UnitVm model);
        Task<UnitVm> DeleteAsync(int id);

        Task<IEnumerable<SelectListItem>> GetDropDownAsync(int? selected = 0, UnitType? type = null, int? id = null);
        Task<IEnumerable<SelectListItem>> GetRegionDropdownAsync(int? selected = 0);
        Task<IEnumerable<SelectListItem>> GetSectorDropdownAsync(int? selected = 0, int? id = null);
        Task<IEnumerable<SelectListItem>> GetBattalionDropdownAsync(int? selected = 0, int? id = null);
        //Task<SelectList> GetSelectAsync(int? selected = 0);
    }
}