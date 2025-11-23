using DataTables.Mvc;
using IDIMAdmin.Entity;
using IDIMAdmin.Extentions;
using IDIMAdmin.Extentions.Collections.Select2;
using IDIMAdmin.Extentions.Exceptions;
using IDIMAdmin.Models.User;
using IDIMAdmin.Services.User;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDIMAdmin.Controllers.User
{
	public class UserController : BaseController
    {
        protected IApplicationService ApplicationService { get; set; }
        protected IDeviceService DeviceService { get; set; }
        protected IMenuService MenuService { get; set; }
        protected IUserService UserService { get; set; }
        protected IUserRolePermissionService UserRolePermissionService { get; set; }
        protected IRoleService RoleService { get; set; }
        protected IActivityLogService ActivityLogService { get; set; }
       


        public UserController(IApplicationService applicationService,
            IDeviceService deviceService,
            IMenuService menuService,
            IUserService userService,
            IUserRolePermissionService userRolePermissionService,
            IRoleService roleService,
            IActivityLogService activityLogService) : base(activityLogService)
        {
            ApplicationService = applicationService;
            DeviceService = deviceService;
            MenuService = menuService;
            UserService = userService;
            UserRolePermissionService = userRolePermissionService;
            RoleService = roleService;
            ActivityLogService = activityLogService;
           
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        #region list, create, detail, edit & delete
        public async Task<ActionResult> List()
        {
            var model = new UserSearchVm
            {
                ApplicationDropdown = await ApplicationService.GetDropDownAsync()
            };

            return View(model);
        }

        public async Task<ActionResult> _List([ModelBinder(typeof(DataTablesBinder))] 
            IDataTablesRequest requestModel, UserSearchVm filter)
        {
            var users = await UserService.GetByAsync(requestModel, filter);

            return Json(users, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Detail(int? id)
        {
            int.TryParse(id.ToString(), out int userId);
            var user = await UserService.GetByIdAsync(userId);
            if (user == null)
                return HttpNotFound();

            var menus = await MenuService.GetByUserIdAsync(user.UserId);
            menus = menus.Where(e => e.Menus.Any(m => m.IsAssigned)).Select(e => new MenuGroupByVm
            {
                ApplicationId = e.ApplicationId,
                ApplicationName = e.ApplicationName,
                Menus = e.Menus.Where(m => m.IsAssigned).Select(l => l).ToList()
            }).ToList();

            var model = new UserDetailVm
            {
                User = user,
                Menus = menus,
                Applications = await ApplicationService.GetByUserIdAsync(user.UserId),
                //Devices = await DeviceService.GetByUserIdAsync(user.UserId),
            };

            return View(model);
        }

        public async Task<ActionResult> Create()
        {
            var model = new RegisterVm();
            model.UserTypeList = await UserService.GetDropDownAsync();
            model.UnitList= await UserService.GetUnitDropDownAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterVm model)
        {
            Message message;

            try
            {
                if (ModelState.IsValid)
                {
                    // Convert list to comma-separated string
                    var uniteIdsString = string.Join(",", model.SelectedUniteIds);

                    var user = new UserVm
                    {
                        ArmyId = model.ArmyId,
                        Username = model.Username,
                        Password = model.Password,
                        Phone = model.Phone,
                        PersonnelCode = model.PersonnelCode,
                        UserType = model.UserType,
                        UniteList = uniteIdsString,
                        Email = model.Email,
                        IsActive = model.IsActive,
                        IsAll = model.IsAll
                    };

                    await UserService.InsertAsync(user);

                    ModelState.Clear();
                    model = new RegisterVm();
                    model.UserTypeList = await UserService.GetDropDownAsync();
                    model.UnitList = await UserService.GetUnitDropDownAsync();

                    message = Messages.Success(MessageType.Create.ToString());

                }
                else
                {
                    message = Messages.InvalidInput(MessageType.Create.ToString());
                }
            }
            catch (Exception exception)
            {
                message = Messages.Failed(MessageType.Create.ToString(), exception.Message);
                model.UserTypeList = await UserService.GetDropDownAsync();
                model.UnitList = await UserService.GetUnitDropDownAsync();
            }

            ViewBag.Message = message;

            return View(model);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            
            int.TryParse(id.ToString(), out int userId);
            var user = await UserService.GetByIdAsync(userId);
            if (user == null)
                return HttpNotFound();

            // 2️⃣ Convert saved comma-separated unit IDs (e.g. "1,2,3") to List<int>
            List<int> selectedUnitIds = new List<int>();
            if (!string.IsNullOrEmpty(user.UniteList))
            {
                selectedUnitIds = user.UniteList
                    .Split(',')
                    .Select(x => int.TryParse(x, out var val) ? val : 0)
                    .Where(x => x > 0)
                    .ToList();
            }

            // 3️⃣ Get full unit dropdown list
            var allUnits = await UserService.GetUnitDropDownAsync(id);
            // Expecting this returns List<SelectListItem>

            // 4️⃣ Mark already selected units
            var unitList = allUnits
               .Select(u => new SelectListItem
               {
                   Value = u.Value,
                   Text = u.Text,
                   Selected = selectedUnitIds.Contains(Convert.ToInt32(u.Value))
               })
               .ToList();
            // 5️⃣ Send lists to view
            ViewBag.UserTypeList = await UserService.GetDropDownAsync(id);
            ViewBag.UniteList = unitList;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserVm model, List<int> SelectedUnitIds)
        {
            Message message;

            try
            {
                if (ModelState.IsValid)
                {
                    var uniteIdsString = string.Join(",", SelectedUnitIds);
                    model.UniteList = uniteIdsString;
                    await UserService.UpdateAsync(model);

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
            ViewBag.UserTypeList = await UserService.GetDropDownAsync(model.UserId);
            ViewBag.UniteList = await UserService.GetUnitDropDownAsync(model.UserId);

            ViewBag.Message = message;

            return View(model);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            int.TryParse(id.ToString(), out int userId);
            var user = await UserService.GetByIdAsync(userId);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            Message message;

            var user = await UserService.GetByIdAsync(id);
            if (user == null)
                return HttpNotFound();

            try
            {
                await UserService.DeleteAsync(id);
                message = Messages.Failed(MessageType.Delete.ToString());
            }
            catch (Exception exception)
            {
                message = Messages.Failed(MessageType.Delete.ToString(), exception.Message());
            }

            ViewBag.Message = message;

            return RedirectToAction("List");
        }
        #endregion

        #region json
        public async Task<ActionResult> Get(Select2Param id)
        {
            var users = await UserService.GetBySelect2Async(id);

            return Json(users, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region change password
        public async Task<ActionResult> ChangePassword(int id)
        {
            int.TryParse(id.ToString(), out int userId);
            var user = await UserService.GetByIdAsync(userId);
            if (user == null)
                return HttpNotFound();

            var model = new ChangePasswordVm
            {
                UserId = user.UserId,
                Username = user.Username,
                Password = user.Password,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordVm model)
        {
            Message message;

            try
            {
                if (ModelState.IsValid)
                {
                    var user = await UserService.GetByIdAsync(model.UserId);
                    model.Username = user.Username;
                    var modelMap = new UserChangePasswordVm
                    {
                        Username = model.Username,
                        Password = null,
                        NewPassword = model.NewPassword,
                        ReNewPassword = model.ReNewPassword,
                    };
                    var change = await UserService.ChangePasswordAsync(modelMap);

                    ModelState.Clear();
                    model = new ChangePasswordVm
                    {
                        Username = user.Username,
                        UserId = user.UserId,
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
            return View();
        }
        #endregion

        //[Route("user/{id:int}/userrole/assign")]
        public async Task<ActionResult> Assign(int id)
        {
            var user = await UserService.GetByIdAsync(id);
            if (user == null)
                return HttpNotFound();

            var model = new UserRolePermissionVm
            {
                UserId = user.UserId,
                UserName = user.Username,
                Roles = await RoleService.GetByUserIdAsync(id)
            };

            return View(model);
        }

        [HttpPost]
        //[Route("user/{id:int}/userrole/assign")]
        public async Task<ActionResult> Assign(UserRolePermissionVm model)
        {
            Message message;

            try
            {
                if (ModelState.IsValid)
                {
                    await UserRolePermissionService.InsertDeleteAsync(model.UserId, model.Roles);

                    message = Messages.Success("Role Assigned");
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
