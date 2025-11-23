using IDIMAdmin.Models.Admin;
using IDIMAdmin.Services.User;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDIMAdmin.Controllers
{
    public class ActivityLogController : BaseController
    {
        protected IApplicationService ApplicationService { get; set; }
       
        public ActivityLogController(IActivityLogService activityLogService,
            IApplicationService applicationService) : base(activityLogService)
        {

            ActivityLogService = activityLogService;
            ApplicationService = applicationService;

        }
        // GET: ActivityLog
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public async Task<ActionResult> List()
        {
            var model = new ActivityLogSearchVm
            {
                ApplicationDropdown = await ApplicationService.GetDropDownAsync(),
            };

            return View(model);
        }

        public async Task<ActionResult> Detail(int? id)
        {
            int.TryParse(id.ToString(), out int Id);

            var model = await ActivityLogService.GetByUserIdAsync(Id);

            //Convert string to Json object
            JToken jsonObject = JToken.Parse(model.ActivityData);

            if (jsonObject["Password"] != null || jsonObject["password"] != null)
            {
                jsonObject["Password"] = "********";
            }
            // Convert the JSON object back to a formatted string
            model.ActivityData = jsonObject.ToString();

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> LoadData()
        {
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var searchValue = Request.Form.GetValues("userId").FirstOrDefault();
            var searchValues = Request.Form.GetValues("searchValues").FirstOrDefault();
            var applicationId = Request.Form.GetValues("applicationId").FirstOrDefault();
            var startDate = Request.Form.GetValues("startDate").FirstOrDefault();
            var endDate = Request.Form.GetValues("endDate").FirstOrDefault();


            // ✅ Determine if search is numeric or text
            int? parsedUserId = null;
            string userName = null;

            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                if (int.TryParse(searchValue, out int tempId))
                    parsedUserId = tempId; // numeric => UserId
                else
                    userName = searchValue; // text => UserName
            }
            var model = new ActivityLogPaginationSearchVm()
            {
                PageIndex = start != null ? Convert.ToInt32(start) : 0,
                PageSize = length != null ? Convert.ToInt32(length) : 0,
                UserId = parsedUserId,
                UserName = userName,
                SearchValues= searchValues,
                ApplicationId = applicationId != "" ? Convert.ToInt32(applicationId) : (int?)null,
                StartDate = startDate != "" ? DateTime.Parse(startDate) : (DateTime?)null,
                EndDate = endDate != "" ? DateTime.Parse(endDate) : (DateTime?)null,
            };

            try
            {
                var data = await ActivityLogService.GetAllAsync(model);
                return Json(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}