using System;
using System.Threading.Tasks;
using System.Web.Mvc;

using IDIMAdmin.Extentions;
using IDIMAdmin.Models.User;
using IDIMAdmin.Services.User;

namespace IDIMAdmin.Controllers.User
{
	public class UserMenuController : Controller
    {
        protected IApplicationService ApplicationService { get; set; }
        protected IDeviceService DeviceService { get; set; }
        protected IUserService UserService { get; set; }
        protected IMenuService MenuService { get; set; }
        protected IUserPriviledgeService UserPriviledgeService { get; set; }

        public UserMenuController(IApplicationService applicationService,
            IDeviceService deviceService, 
            IMenuService menuService,
            IUserService userService,
            IUserPriviledgeService userPriviledgeService)
        {
            ApplicationService = applicationService;
            DeviceService = deviceService;
            MenuService = menuService;
            UserService = userService;
            UserPriviledgeService = userPriviledgeService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("User");
        }

        public new async Task<ActionResult> User()
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
        public async Task<ActionResult> User(UserSearchVm model)
        {
            //model.ApplicationDropdown = await ApplicationService.GetDropDownAsync(model.ApplicationId);
            //model.DeviceDropdown = await DeviceService.GetDropDownAsync(model.DeviceId);
            //model.Users = await UserService.GetUserByFilterAsync(model);

            return View(model);
        }

        //public ActionResult Assign()
        //{
        //    return View(new UserRegimentVm());
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Assign(UserRegimentVm model)
        //{
        //    JsonMessage message;

        //    try
        //    {
        //        int.TryParse(model.UserId.ToString(), out int userId);
        //        var user = await UserService.GetByIdAsync(userId);
        //        if (user == null)
        //            return Json(JsonMessages.Failed("User not found.", JsonMessageType.NotFound),
        //                JsonRequestBehavior.AllowGet);

        //        var regiment = await GeneralInformationService.GetByIdAsync(model.ArmyId);
        //        if (regiment == null)
        //            return Json(JsonMessages.Failed("Regiment not found.", JsonMessageType.NotFound),
        //                JsonRequestBehavior.AllowGet);

        //        user.ArmyId = model.ArmyId;
        //        await UserService.UpdateAsync(user);

        //        message = JsonMessages.Success("Assign successfully", JsonMessageType.Failed);
        //    }
        //    catch (Exception exception)
        //    {
        //        message = JsonMessages.Failed(exception.Message(), JsonMessageType.Failed);
        //    }

        //    return Json(message, JsonRequestBehavior.AllowGet);
        //}

        public async Task<ActionResult> Assign(int id)
        {
            var user = await UserService.GetByIdAsync(id);
            if (user == null)
                return HttpNotFound();

            var model = new UserMenuAssignVm
            {
                UserId = user.UserId,
                RegimentNo = user.RegimentNo,
                Username = user.Username,
                //Menus = await MenuService.GetByUserApplicationAsync(id)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Assign(UserMenuAssignVm model)
        {
            Message message;

            try
            {
                if (ModelState.IsValid)
                {
                    //await UserPriviledgeService.InsertDeleteAsync(model.UserId, model.Menus);

                    message = Messages.Success("Menus assigne");
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