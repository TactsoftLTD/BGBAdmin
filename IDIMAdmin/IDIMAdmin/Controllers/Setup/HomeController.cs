using System.Threading.Tasks;
using System.Web.Mvc;

using IDIMAdmin.Services.User;

namespace IDIMAdmin.Controllers.Setup
{
	public class HomeController : BaseController
    {
        protected IApplicationService ApplicationService { get; set; }
        protected IActivityLogService ActivityLogService { get; set; }

        public HomeController(IApplicationService applicationService,
            IActivityLogService activityLogService) : base(activityLogService)
        {
            ApplicationService = applicationService;
            ActivityLogService = activityLogService;
        }

        public async Task<ActionResult> Index()
        {
            var applications = await ApplicationService.GetAllPublishedAsync();

            return View(applications);
        }
    }
}