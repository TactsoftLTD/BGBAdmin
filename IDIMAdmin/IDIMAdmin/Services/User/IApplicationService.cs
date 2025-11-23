using IDIMAdmin.Models;
using IDIMAdmin.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDIMAdmin.Services.User
{
    public interface IApplicationService
    {
        Task<IList<ApplicationVm>> GetAllAsync();
        Task<IList<ApplicationVm>> GetAllPublishedAsync();
        Task<ApplicationVm> GetByIdAsync(int id);
        Task<IList<ApplicationVm>> GetByUserIdAsync(int userId);
        Task<ApplicationVm> InsertAsync(ApplicationVm model);
        Task<ApplicationVm> UpdateAsync(ApplicationVm model);
        Task<ApplicationVm> DeleteAsync(int id);
        Task<IEnumerable<SelectListItem>> GetDropDownAsync(int? selected = 0);
        Task<LandingVm> GetApplicationsAndSliderImages();
        //Task<IList<ApplicationAssignVm>> GetAssignByUserIdAsync(int userId);
    }
}