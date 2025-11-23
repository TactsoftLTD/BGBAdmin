using System.Threading.Tasks;
using System.Web.Mvc;

using IDIMAdmin.Services.User;

namespace IDIMAdmin.Controllers
{
	public class LandingController : Controller
    {
        protected IApplicationService ApplicationService { get; set; }

        public LandingController(IApplicationService applicationService)
        {
            ApplicationService = applicationService;
        }

        public async Task<ActionResult> Index()
        {
            var appswimages = await ApplicationService.GetApplicationsAndSliderImages();

            return View(appswimages);
        }
    }
}