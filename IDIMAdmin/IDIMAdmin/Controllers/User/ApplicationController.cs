using IDIMAdmin.Extentions;
using IDIMAdmin.Models.User;
using IDIMAdmin.Services.User;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDIMAdmin.Controllers.User
{
    public class ApplicationController : BaseController
    {
        protected IApplicationService ApplicationService { get; set; }
        protected IMenuService MenuService { get; set; }
        protected IUserService UserService { get; set; }
        protected IActivityLogService ActivityLogService { get; set; }

        public ApplicationController(
            IApplicationService applicationService,
            IMenuService menuService,
            IUserService userService,
            IActivityLogService activityLogService) : base(activityLogService)
        {
            ApplicationService = applicationService;
            MenuService = menuService;
            UserService = userService;
            ActivityLogService = activityLogService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        #region list, detail, create, edit & delete
        public async Task<ActionResult> List()
        {
            var applications = await ApplicationService.GetAllAsync();

            return View(applications);
        }

        public async Task<ActionResult> Detail(int? id)
        {
            int.TryParse(id.ToString(), out int applicationId);
            var model = new ApplicationDetailVm
            {
                Application = await ApplicationService.GetByIdAsync(applicationId),
                Menus = await MenuService.GetAllAsync(applicationId),
                //Users = await UserService.GetByApplicationIdAsync(applicationId)
            };

            return View(model);
        }

        public ActionResult Create()
        {
            return View(new ApplicationVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ApplicationVm model)
        {
            Message message;
            if (ModelState.IsValid)
            {
                try
                {
                    await ApplicationService.InsertAsync(model);
                    ModelState.Clear();
                    model = new ApplicationVm();
                    message = Messages.Success(MessageType.Create.ToString());
                }
                catch (Exception exception)
                {
                    message = Messages.Failed(MessageType.Create.ToString(), exception.Message);
                }
            }
            else
            {
                message = Messages.InvalidInput(MessageType.Create.ToString());
            }

            ViewBag.Message = message;

            return View(model);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var model = await ApplicationService.GetByIdAsync(id);
            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ApplicationVm model)
        {
            Message message;
            if (ModelState.IsValid)
            {
                try
                {
                    await ApplicationService.UpdateAsync(model);
                    message = Messages.Success(MessageType.Update.ToString());
                }
                catch (Exception exception)
                {
                    message = Messages.Failed(MessageType.Update.ToString(), exception.Message);
                }
            }
            else
            {
                message = Messages.InvalidInput(MessageType.Update.ToString());
            }

            ViewBag.Message = message;

            return View(model);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            int.TryParse(id.ToString(), out int userId);
            var model = await ApplicationService.GetByIdAsync(userId);
            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            Message message;
            try
            {
                await ApplicationService.DeleteAsync(id);
                message = Messages.Success(MessageType.Delete.ToString());
            }
            catch (Exception exception)
            {
                message = Messages.Failed(MessageType.Delete.ToString(), exception);
            }

            ViewBag.Message = message;

            return RedirectToAction("List");
        }
        #endregion

        #region json
        /// <summary>
        /// All application data 
        /// </summary>
        /// <returns>All application list in JSON format</returns>
        public async Task<ActionResult> Get()
        {
            var applications = await ApplicationService.GetAllAsync();

            var data = new
            {
                data = applications
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region User
        /// <summary>
        /// Get user list by application identitfier
        /// </summary>
        /// <param name="id">Application identifier</param>
        /// <returns></returns>
        public new async Task<ActionResult> User(int? id)
        {
            int.TryParse(id.ToString(), out int applicationId);
            var model = new ApplicationDetailVm
            {
                Application = await ApplicationService.GetByIdAsync(applicationId),
                Users = await UserService.GetByApplicationIdAsync(applicationId)
            };

            return View(model);
        }
        #endregion

        #region application menu
        public async Task<ActionResult> Menu(int? id)
        {
            int.TryParse(id.ToString(), out int menuId);
            var model = new ApplicationDetailVm
            {
                Application = await ApplicationService.GetByIdAsync(menuId),
                Menus = await MenuService.GetAllAsync(menuId)
            };

            return View(model);
        }
        #endregion
    }
}