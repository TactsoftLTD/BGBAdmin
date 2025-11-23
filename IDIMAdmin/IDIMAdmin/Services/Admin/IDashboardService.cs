using System.Collections.Generic;
using System.Threading.Tasks;
using IDIMAdmin.Models.Dashboard;

namespace IDIMAdmin.Services.Admin
{
    public interface IDashboardService
    {
        Task<DashboardVm> GetAll();
        Task<List<ApplicationDetailVm>> Application();
        Task<List<RegionDetailVm>> Region();
    }
}