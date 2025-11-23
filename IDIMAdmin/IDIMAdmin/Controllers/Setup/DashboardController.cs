using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

using IDIMAdmin.Services.Admin;
using IDIMAdmin.Services.User;

namespace IDIMAdmin.Controllers.Setup
{
	public class DashboardController : BaseController
    {
        protected IDashboardService DashboardService { get; set; }
   
        public DashboardController(IDashboardService dashboardService,
            IActivityLogService activityLogService) : base(activityLogService)
        {
            DashboardService = dashboardService;
            ActivityLogService = activityLogService;
        }

        public async Task<ActionResult> Index()
        {
            var model = await DashboardService.GetAll();

            return View(model);
        }

        public ActionResult About()
        {
            IDictionary<int, string> dic = new Dictionary<int, string>();
            Dictionary<string, object> dict = new Dictionary<string, object>();

            return View();
        }

        public async Task<ActionResult> Application()
        {
            var data = await DashboardService.Application();

            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
    }
}