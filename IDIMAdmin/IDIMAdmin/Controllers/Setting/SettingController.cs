using IDIMAdmin.Services.User;

using System.Web.Mvc;

namespace IDIMAdmin.Controllers.Setting
{
	public class SettingController : BaseController
    {

        public SettingController(IActivityLogService activityLogService) : base(activityLogService)
        {
            ActivityLogService = activityLogService;
        }
        // GET: Setting
        public ActionResult Index()
        {
            return View();
        }
    }
}