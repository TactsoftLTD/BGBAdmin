using System;
using System.Threading.Tasks;
using System.Web.Mvc;

using IDIMAdmin.Extentions.Data;
using IDIMAdmin.Extentions.Exceptions;
using IDIMAdmin.Models.User;
using IDIMAdmin.Services.Setup;
using IDIMAdmin.Services.User;

namespace IDIMAdmin.Controllers.User
{
	public class UserRegimentController : BaseController
    {
        protected IGeneralInformationService GeneralInformationService { get; set; }
        protected IUserService UserService { get; set; }
        protected IActivityLogService ActivityLogService { get; set; }
        public UserRegimentController(IGeneralInformationService generalInformationService,
            IUserService userService,
            IActivityLogService activityLogService) : base(activityLogService)
        {
            GeneralInformationService = generalInformationService;
            UserService = userService;
            ActivityLogService = activityLogService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Assign");
        }

        public ActionResult Assign()
        {
            return View(new UserRegimentVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Assign(UserRegimentVm model)
        {
            JsonMessage message;

            try
            {
                int.TryParse(model.UserId.ToString(), out int userId);
                var user = await UserService.GetByIdAsync(userId);
                if (user == null)
                    return Json(JsonMessages.Failed("User not found.", JsonMessageType.NotFound),
                        JsonRequestBehavior.AllowGet);

                var regiment = await GeneralInformationService.GetByIdAsync(model.ArmyId);
                if (regiment == null)
                    return Json(JsonMessages.Failed("Regiment not found.", JsonMessageType.NotFound),
                        JsonRequestBehavior.AllowGet);

                user.ArmyId = model.ArmyId;
                await UserService.UpdateAsync(user);

                message = JsonMessages.Success("Assign successfully", JsonMessageType.Failed);
            }
            catch (Exception exception)
            {
                message = JsonMessages.Failed(exception.Message(), JsonMessageType.Failed);
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }
    }
}