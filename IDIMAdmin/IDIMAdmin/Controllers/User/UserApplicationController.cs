using System;
using System.Threading.Tasks;
using System.Web.Mvc;

using IDIMAdmin.Extentions;
using IDIMAdmin.Extentions.Session;
using IDIMAdmin.Models.User;
using IDIMAdmin.Services.User;

namespace IDIMAdmin.Controllers.User
{
	public class UserApplicationController : BaseController
    {
        protected IApplicationService ApplicationService { get; set; }
        protected IDeviceService DeviceService { get; set; }
        protected IUserService UserService { get; set; }
        protected IMenuService MenuService { get; set; }
        protected IUserApplicationService UserApplicationService { get; set; }
        protected IUserPriviledgeService UserPriviledgeService { get; set; }
        protected int UserId { get; set; }

        public UserApplicationController(IApplicationService applicationService,
            IDeviceService deviceService,
            IMenuService menuService,
            IUserService userService,
            IUserApplicationService userApplicationService,
            IUserPriviledgeService userPriviledgeService,
            IActivityLogService activityLogService) : base(activityLogService)
        {
            ApplicationService = applicationService;
            DeviceService = deviceService;
            MenuService = menuService;
            UserService = userService;
            UserApplicationService = userApplicationService;
            UserPriviledgeService = userPriviledgeService;
            UserId = UserExtention.GetUserId();
            ActivityLogService = activityLogService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("User");
        }
        [HttpGet]
		public ActionResult User()
		{
           
			var model = new UserSearchVm
			{
				//ApplicationDropdown = await ApplicationService.GetDropDownAsync(),
				//DeviceDropdown = await DeviceService.GetDropDownAsync(),
				//Users = await UserService.GetUserByFilterAsync()
			};

			return View(model);
		}

		[HttpPost]
		public ActionResult User(UserSearchVm model)
		{
			//model.ApplicationDropdown = await ApplicationService.GetDropDownAsync(model.ApplicationId);
			//model.DeviceDropdown = await DeviceService.GetDropDownAsync(model.DeviceId);
			//model.Users = await UserService.GetUserByFilterAsync(model);

			return View(model);
		}

		[Route("user/{id:int}/application/assign")]
        public async Task<ActionResult> Assign(int id)
        {
            var user = await UserService.GetByIdAsync(id);
            var model = new UserApplicationAssignVm
            {
                UserId = user.UserId,
                RegimentNo = user.RegimentNo,
                Username = user.Username,
                //Applications = await ApplicationService.GetAssignByUserIdAsync(id),
            };

            return View(model);
        }

        [HttpPost]
        [Route("user/{id:int}/application/assign")]
        public async Task<ActionResult> Assign(UserApplicationAssignVm model)
        {
            Message message;

            try
            {
                if (ModelState.IsValid)
                {
                    //await UserApplicationService.InsertDeleteAsync(model.UserId, model.Applications);

                    message = Messages.Success("Applications assigne");
                }
                else
                {
                    message = Messages.InvalidInput(MessageType.Create.ToString());
                }
            }
            catch (Exception exception)
            {
                message = Messages.Failed(MessageType.Create.ToString(), exception.Message);
            }

            ViewBag.Message = message;

            return View(model);
        }
    }
}