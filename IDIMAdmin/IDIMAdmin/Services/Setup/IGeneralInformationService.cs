using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using IDIMAdmin.Entity;
using IDIMAdmin.Extentions.Collections.Select2;
using IDIMAdmin.Models.Setup;

namespace IDIMAdmin.Services.Setup
{
    public interface IGeneralInformationService
    {
        IQueryable<GeneralInformation> GetAll();
        Task<List<GeneralInformationVm>> GetAllAsync(GeneralInformationSearchVm filter = null);
        Task<GeneralInformationVm> GetByIdAsync(int? id);
        Task<IEnumerable<SelectListItem>> GetDropdownAsync(int? selected = 0);
        Task<Select2PagedResult> GetBySelect2Async(Select2Param param);
    }
}