using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

using IDIMAdmin.Extentions;
using IDIMAdmin.Extentions.Session;
using IDIMAdmin.Models.User;
using IDIMAdmin.Services.Setup;
using IDIMAdmin.Services.User;

namespace IDIMAdmin.Controllers.Setting
{
	public class ProfileController : BaseController
    {
        protected IApplicationService ApplicationService { get; set; }
        protected IDeviceService DeviceService { get; set; }
        protected IGeneralInformationService GeneralInformationService { get; set; }
        protected IMenuService MenuService { get; set; }
        protected IUserService UserService { get; set; }


        public ProfileController(IApplicationService applicationService,
            IDeviceService deviceService,
            IGeneralInformationService generalInformationService,
            IMenuService menuService,
            IUserService userService,
            IActivityLogService activityLogService) : base(activityLogService)
        {
            ApplicationService = applicationService;
            DeviceService = deviceService;
            GeneralInformationService = generalInformationService;
            MenuService = menuService;
            UserService = userService;
            ActivityLogService = activityLogService;
        }

        public async Task<ActionResult> Index()
        {
            var userData = UserExtention.Get<UserInformation>(nameof(UserInformation));

            if (userData == null)
                return Redirect("~/");

            var user = await UserService.GetByIdAsync(userData.UserId);
            if (user == null)
                return HttpNotFound();

            var menus = await MenuService.GetByUserIdAsync(user.UserId);
            menus = menus.Where(e => e.Menus.Any(m => m.IsAssigned)).Select(e => new MenuGroupByVm
            {
                ApplicationId = e.ApplicationId,
                ApplicationName = e.ApplicationName,
                Menus = e.Menus.Where(m => m.IsAssigned).Select(l => l).ToList()
            }).ToList();

            var profile = new UserProfile
            {
                User = user,
                Regiment = await GeneralInformationService.GetByIdAsync(user.ArmyId),
                Applications = await ApplicationService.GetByUserIdAsync(user.UserId),
                Menus = menus,
                //Devices = await DeviceService.GetByUserIdAsync(user.UserId)
            };

            return View(profile);
        }

        public ActionResult ChangePassword()
        {
            return View(new UserChangePasswordVm());
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(UserChangePasswordVm model)
        {
            Message message;

            try
            {
                if (ModelState.IsValid)
                {
                    var user = UserExtention.Get<UserInformation>(nameof(UserInformation));
                    model.Username = user.Username;
                    var change = await UserService.ChangePasswordAsync(model);

                    ModelState.Clear();
                    model = new UserChangePasswordVm
                    {
                        Username = user.Username
                    };

                    message = Messages.Success(MessageType.Update.ToString());
                }
                else
                {
                    message = Messages.InvalidInput(MessageType.Update.ToString());
                }
            }
            catch (Exception exception)
            {
                message = Messages.Failed(MessageType.Update.ToString(), exception.Message);
            }

            ViewBag.Message = message;

            return View(model);
        }
    }
}