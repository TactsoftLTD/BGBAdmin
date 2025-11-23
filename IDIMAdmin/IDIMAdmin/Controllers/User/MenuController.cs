using DataTables.Mvc;
using IDIMAdmin.Extentions;
using IDIMAdmin.Models.User;
using IDIMAdmin.Services.User;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDIMAdmin.Controllers.User
{
    public class MenuController : BaseController
    {
        protected IApplicationService ApplicationService { get; set; }
        protected IMenuService MenuService { get; set; }
        protected IActivityLogService ActivityLogService { get; set; }

        public MenuController(IApplicationService applicationService,
            IMenuService menuService,
            IActivityLogService activityLogService) : base(activityLogService)
        {
            ApplicationService = applicationService;
            MenuService = menuService;
            ActivityLogService = activityLogService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public async Task<ActionResult> List()
        {
            var menus = new MenuSearchVm
            {
                ApplicationDropdown = await ApplicationService.GetDropDownAsync()
            };

            return View(menus);
        }

        public async Task<ActionResult> _List([ModelBinder(typeof(DataTablesBinder))]
            IDataTablesRequest requestModel, MenuSearchVm filter)
        {
            var menus = await MenuService.GetByAsync(requestModel, filter);

            return Json(menus, JsonRequestBehavior.AllowGet);
        }

        //public async Task<ActionResult> Detail(int id)
        //{
        //    var application = await MenuService.GetByIdAsync(id);
        //    var model = new MenuDetailVm
        //    {
        //        Menu = application,
        //    };

        //    return View(model);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id">Menu Identifier</param>
        ///// <returns></returns>
        //public new async Task<ActionResult> User(int id)
        //{
        //    var model = new MenuDetailVm
        //    {
        //        Menu = await MenuService.GetByIdAsync(id),
        //        Users = await UserService.GetByMenuIdAsync(id)
        //    };

        //    return View(model);
        //}

        public async Task<ActionResult> Create()
        {
            var menu = new MenuVm
            {
                ApplicationDropdown = await ApplicationService.GetDropDownAsync()
            };

            return View(menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MenuVm model)
        {
            Message message;
            if (ModelState.IsValid)
            {
                try
                {
                    await MenuService.InsertAsync(model);
                    ModelState.Clear();
                    model = new MenuVm();
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

            model.ApplicationDropdown = await ApplicationService.GetDropDownAsync(model.ApplicationId);

            ViewBag.Message = message;

            return View(model);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var model = await MenuService.GetByIdAsync(id);
            if (model == null)
                return HttpNotFound();

            model.ApplicationDropdown = await ApplicationService.GetDropDownAsync(model.ApplicationId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MenuVm model)
        {
            Message message;
            if (ModelState.IsValid)
            {
                try
                {
                    await MenuService.UpdateAsync(model);
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

            model.ApplicationDropdown = await ApplicationService.GetDropDownAsync(model.ApplicationId);
            ViewBag.Message = message;

            return View(model);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var model = await MenuService.GetByIdAsync(id);
            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            Message message;
            int.TryParse(id.ToString(), out int menuId);
            var menu = await MenuService.GetByIdAsync(menuId);
            if (menu == null)
                return HttpNotFound();

            try
            {
                await MenuService.DeleteAsync(menu.MenuId);
                message = Messages.Success(MessageType.Delete.ToString());
            }
            catch (Exception exception)
            {
                message = Messages.Failed(MessageType.Delete.ToString(), exception);
            }

            ViewBag.Message = message;

            return RedirectToAction("List");
        }

        public ActionResult Generate()
        {
            var controllers = ClassExtentions.GetControllerNames();

            var model = controllers.ToList().Except(DefaultData.ExcludeMenu).Select(e => new MenuVm
            {
                ApplicationId = DefaultData.ApplicationId,
                Title = e,
                ControllerName = e,
                MenuType = MenuType.Information,
                Priority = 1,
            }).ToList();

            //var data = model.Select(e => $"new MenuVm{{Title = \"{e.ControllerName}\",ControllerName = \"{e.ControllerName}\", MenuType = {(int) MenuType.Information}, Priority = 1, Icon = \"\"}}").ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerateDynamic()
        {
            var controllers = ClassExtentions.GetControllerNames();
            controllers = controllers.Except(DefaultData.ExcludeMenu).ToList();

            var model = new MenuGenerateVm
            {
                Menus = controllers.Select(e => new MenuVm { ControllerName = e, Title = e }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GenerateDynamic(MenuGenerateVm model)
        {
            Message message;

            try
            {
                await MenuService.InsertAllAsync(model);
                message = Messages.Success("Generate");
            }
            catch (Exception e)
            {
                message = Messages.Failed(MessageType.Create.ToString(), e.Message);
            }

            ViewBag.Message = message;

            return View(model);
        }

        public ActionResult GenerateStatic()
        {
            var controllers = MenuService.AdminMenuData();

            var model = new MenuGenerateVm
            {
                Menus = controllers
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GenerateStatic(MenuGenerateVm model)
        {
            Message message;

            try
            {
                await MenuService.InsertAllAsync(model);
                message = Messages.Success("Generate");
            }
            catch (Exception e)
            {
                message = Messages.Failed(MessageType.Create.ToString(), e.Message);
            }

            ViewBag.Message = message;

            return View(model);
        }

        /// <summary>
        /// All menu data 
        /// </summary>
        /// <returns>All menu in JSON format</returns>
        public async Task<ActionResult> Get()
        {
            var menus = await MenuService.GetAllAsync();

            return Json(new { data = menus }, JsonRequestBehavior.AllowGet);
        }
    }
}