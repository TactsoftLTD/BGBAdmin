using IDIMAdmin.Services.User;

using System.Web.Mvc;

namespace IDIMAdmin.Controllers.Setting
{
	public class HelpController : BaseController
    {
    
        public HelpController(IActivityLogService activityLogService) : base(activityLogService)
        {
            ActivityLogService = activityLogService;
        }
        // GET: Help
        public ActionResult Index()
        {
            return View();
        }
    }
}