using DataTables.Mvc;
using IDIMAdmin.Extentions;
using IDIMAdmin.Models.User;
using IDIMAdmin.Services.User;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDIMAdmin.Controllers.User
{
    public class RoleController : BaseController
    {
        protected IApplicationService ApplicationService { get; set; }
        protected IRoleService RoleService { get; set; }
        protected IRoleMenuPermissionService RoleMenuPermissionService { get; set; }
        protected IMenuService MenuService { get; set; }
        protected IActivityLogService ActivityLogService { get; set; }

        public RoleController(IApplicationService applicationService,
            IRoleService roleService,
            IRoleMenuPermissionService roleMenuPermissionService,
            IMenuService menuService,
            IActivityLogService activityLogService) : base(activityLogService)
        {
            ApplicationService = applicationService;
            RoleService = roleService;
            RoleMenuPermissionService = roleMenuPermissionService;
            MenuService = menuService;
            ActivityLogService = activityLogService;
        }

        // GET: Role
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public async Task<ActionResult> List()
        {
            var applications = new RoleSearchVm
            {
                ApplicationDropdown = await ApplicationService.GetDropDownAsync()
            };

            return View(applications);
        }

        public async Task<ActionResult> Create()
        {
            var applications = new RoleVm
            {
                ApplicationDropdown = await ApplicationService.GetDropDownAsync()
            };

            return View(applications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleVm model)
        {
            Message message;
            if (ModelState.IsValid)
            {
                try
                {
                    await RoleService.InsertAsync(model);
                    ModelState.Clear();
                    model = new RoleVm();
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

        public async Task<ActionResult> _List([ModelBinder(typeof(DataTablesBinder))]
            IDataTablesRequest requestModel, RoleSearchVm filter)
        {
            var roles = await RoleService.GetByAsync(requestModel, filter);

            return Json(roles, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var model = await RoleService.GetByIdAsync(id);
            if (model == null)
                return HttpNotFound();

            model.ApplicationDropdown = await ApplicationService.GetDropDownAsync(model.ApplicationId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleVm model)
        {
            Message message;
            if (ModelState.IsValid)
            {
                try
                {
                    await RoleService.UpdateAsync(model);
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
            var model = await RoleService.GetByIdAsync(id);
            if (model == null)
                return HttpNotFound();


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            Message message;
            int.TryParse(id.ToString(), out int roleId);
            var role = await RoleService.GetByIdAsync(roleId);
            if (role == null)
                return HttpNotFound();

            try
            {
                await RoleService.DeleteAsync(role.RoleId);
                message = Messages.Success(MessageType.Delete.ToString());
            }
            catch (Exception exception)
            {
                message = Messages.Failed(MessageType.Delete.ToString(), exception);
            }

            ViewBag.Message = message;

            return RedirectToAction("List");
        }
        // Assign Menu by RoleId
        public async Task<ActionResult> MenuAssign(int id)
        {
            var role = await RoleService.GetRoleInfo(id);
            if (role == null)
                return HttpNotFound();

            var model = new RoleMenuPermissionVm
            {
                RoleName = role.Name,
                ApplicationName = role.ApplicationName,
                ApplicationId = role.ApplicationId,
                RoleId = id,
                Menus = await MenuService.GetMenuByApplication(role.ApplicationId, id)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> MenuAssign(RoleMenuPermissionVm model)
        {
            Message message;

            try
            {
                if (ModelState.IsValid)
                {
                    await RoleMenuPermissionService.InsertDeleteAsync(model.RoleId, model.Menus);

                    message = Messages.Success("Menu Assigned");
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