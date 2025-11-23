using AutoMapper;
using DataTables.Mvc;
using IDIMAdmin.Entity;
using IDIMAdmin.Extentions;
using IDIMAdmin.Extentions.Session;
using IDIMAdmin.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDIMAdmin.Services.User
{
    public class MenuService : IMenuService
    {
        protected IDIMDBEntities Context { get; set; }
        protected IMapper Mapper { get; set; }

        public MenuService(IMapper mapper)
        {
            Context = new IDIMDBEntities();
            Mapper = mapper;
        }

        #region private
        private IQueryable<MenuGroupByVm> MenuByApplication()
        {
            var menuGroups = Context.Applications
                .Include(e => e.Menus)
                .GroupBy(e => new { e.ApplicationId, e.ApplicationName })
                .Select(g => new MenuGroupByVm
                {
                    ApplicationId = g.Key.ApplicationId,
                    ApplicationName = g.Key.ApplicationName,
                    Menus = g.SelectMany(m => m.Menus.Select(l => new MenuAssignVm
                    {
                        MenuId = l.MenuId,
                        Title = l.Title,
                        ControllerName = l.ControllerName,
                        ApplicationId = l.ApplicationId
                    })).ToList()
                }).OrderBy(e => e.ApplicationName);

            return menuGroups;
        }

        private IQueryable<RolePrivilege> UserMenuByUserId(int userId)
        {
            return (from up in Context.UserPrivileges
                    join rp in Context.RolePrivileges on up.RoleId equals rp.RoleId
                    where up.UserId == userId
                    select rp);
        }
        #endregion

        #region menu data
        public List<MenuVm> AdminMenuData()
        {
            var menus = new List<MenuVm>
            {
                new MenuVm{Title = "Dashboard",ControllerName = "Dashboard", MenuType = MenuType.Main, Priority = 1, Icon = "dashboard"},
                new MenuVm{Title = "Home",ControllerName = "Home", MenuType = MenuType.Main, Priority = 2, Icon = "home"},
                new MenuVm{Title = "Report",ControllerName = "Report", MenuType = MenuType.Main, Priority = 3, Icon = "line-chart"},

                new MenuVm{Title = "User Regiment",ControllerName = "UserRegiment", MenuType = MenuType.Information, Priority = 1, Icon = "plus"},
                new MenuVm{Title = "User Application",ControllerName = "UserApplication", MenuType = MenuType.Information, Priority = 2, Icon = "plus"},
                new MenuVm{Title = "User Menu",ControllerName = "UserMenu", MenuType = MenuType.Information, Priority = 3, Icon = "plus"},

                new MenuVm{Title = "Application",ControllerName = "Application", MenuType = MenuType.Setup, Priority = 1, Icon = "building"},
                new MenuVm{Title = "Menu",ControllerName = "Menu", MenuType = MenuType.Setup, Priority = 2, Icon = "bars"},
                new MenuVm{Title = "Device",ControllerName = "Device", MenuType = MenuType.Setup, Priority = 3, Icon = "desktop"},
                new MenuVm{Title = "User",ControllerName = "User", MenuType = MenuType.Setup, Priority = 4, Icon = "users"},

                new MenuVm{Title = "General Information",ControllerName = "GeneralInformation", MenuType = MenuType.Other, Priority = 1, Icon = "check"},
                new MenuVm{Title = "Unit", ControllerName = "Unit", MenuType = MenuType.Other, Priority = 2, Icon = "flag"},
            };

            return menus.Select(e =>
            {
                e.ControllerName = e.ControllerName.ToLower();
                e.ApplicationId = DefaultData.ApplicationId;
                e.IsPublished = true;
                return e;
            }).ToList();
        }
        #endregion

        public async Task<List<MenuVm>> GetAllAsync(int? applicationId = null)
        {
            var query = Context.Menus
                .AsQueryable();

            if (applicationId.HasValue)
                query = query.Where(e => e.ApplicationId == applicationId);

            var list = await query.ToListAsync();

            return Mapper.Map<List<MenuVm>>(list);
        }

        public async Task<MenuVm> GetByIdAsync(int id)
        {
            var entity = await Context.Menus.FindAsync(id);

            return Mapper.Map<MenuVm>(entity);
        }

        public async Task<MenuVm> InsertAsync(MenuVm model)
        {
            var existing = await Context.Menus
                .FirstOrDefaultAsync(m => m.ControllerName == model.ControllerName && m.ApplicationId == model.ApplicationId);
            if (existing != null)
                throw new ArgumentException($"Menu already exists ");

            var entity = Mapper.Map<Menu>(model);
            entity.CreatedDateTime = DateTime.Now;
            entity.CreatedUser = UserExtention.GetUserId();

            var added = Context.Menus.Add(entity);
            await Context.SaveChangesAsync();

            return Mapper.Map<MenuVm>(added);
        }

        public async Task InsertAllAsync(MenuGenerateVm model)
        {
            var menus = model.Menus;
            var entities = await Context.Menus
                .Where(e => e.ApplicationId == DefaultData.ApplicationId)
                .ToListAsync();

            if (entities.Any())
                throw new ArgumentException("Menu already generated, you can generate only first time.");

            var newMenus = menus.Where(m => !entities.Any(e => string.Equals(m.ControllerName, e.ControllerName, StringComparison.CurrentCultureIgnoreCase))).ToList();

            var addMenus = newMenus.Select(e => new Menu
            {
                ApplicationId = DefaultData.ApplicationId,
                ControllerName = e.ControllerName,
                Title = e.Title,
                IsPublished = true,
                Icon = e.Icon,
                MenuType = (int)e.MenuType,
                Priority = e.Priority,
                CreatedDateTime = DateTime.Now,
                CreatedUser = UserExtention.GetUserId()
            }).ToList();
            Context.Menus.AddRange(addMenus);

            await Context.SaveChangesAsync();
        }

        public async Task<MenuVm> UpdateAsync(MenuVm model)
        {
            var existing = await Context.Menus.FindAsync(model.MenuId);

            if (existing == null)
                throw new ArgumentException($"Menu does not exists.");

            var duplicate = await Context.Menus
                .Where(e => e.MenuId != model.MenuId)
                .FirstOrDefaultAsync(e => e.ControllerName == model.ControllerName && e.ApplicationId == model.ApplicationId);

            if (duplicate != null)
                throw new ArgumentException($"Menu already exists.");

            existing.Title = model.Title;
            existing.ApplicationId = model.ApplicationId;
            existing.ControllerName = model.ControllerName;
            existing.MenuType = (int)model.MenuType;
            existing.Priority = model.Priority;
            existing.Icon = model.Icon;
            existing.IsPublished = model.IsPublished;
            existing.Remark = model.Remark;
            existing.UpdatedDateTime = DateTime.Now;
            existing.UpdatedUser = UserExtention.GetUserId();
            existing.UpdateNo += 1;

            await Context.SaveChangesAsync();

            return Mapper.Map<MenuVm>(existing);
        }

        public async Task<MenuVm> DeleteAsync(int id)
        {
            var existing = await Context.Menus.FindAsync(id);

            if (existing == null)
                throw new ArgumentException($"Menu does not exists.");

            Context.Entry(existing).State = EntityState.Deleted;
            await Context.SaveChangesAsync();

            return Mapper.Map<MenuVm>(existing);
        }

        public async Task InsertDeleteAsync(MenuGenerateVm model)
        {
            var menus = model.Menus;
            var entities = await Context.Menus
                .Where(e => e.ApplicationId == DefaultData.ApplicationId)
                .ToListAsync();

            var matchMenus = menus.Where(m => entities.Any(e => e.ControllerName.ToLower() == m.ControllerName.ToLower())).ToList();
            var newMenus = menus.Where(m => !entities.Any(e => string.Equals(m.ControllerName, e.ControllerName, StringComparison.CurrentCultureIgnoreCase))).ToList();
            var notMatchMenus = entities.Where(e => menus.All(m => !string.Equals(m.ControllerName, e.ControllerName, StringComparison.CurrentCultureIgnoreCase))).ToList();

            var addMenus = newMenus.Select(e => new Menu
            {
                ApplicationId = DefaultData.ApplicationId,
                ControllerName = e.ControllerName,
                Title = e.Title,
                IsPublished = true,
                Icon = e.Icon,
                MenuType = (int)e.MenuType,
                Priority = e.Priority,
                CreatedDateTime = DateTime.Now,
                CreatedUser = UserExtention.GetUserId()
            }).ToList();
            Context.Menus.AddRange(addMenus);

            var deleteMenus = notMatchMenus;
            Context.Menus.RemoveRange(deleteMenus);

            await Context.SaveChangesAsync();
        }

        public async Task<DataTablesResponse> GetByAsync(IDataTablesRequest requestModel, MenuSearchVm filter)
        {
            if (filter == null)
                filter = new MenuSearchVm();

            var query = Context.Menus.AsQueryable();
            var totalCount = query.Count();

            query = query
                .Where(x => (string.IsNullOrEmpty(filter.Title) || x.Title.Contains(filter.Title)) &&
                            (string.IsNullOrEmpty(filter.ControllerName) || x.ControllerName.Contains(filter.ControllerName)) &&
                            (!filter.ApplicationId.HasValue || x.ApplicationId == filter.ApplicationId.Value)).
                            OrderBy(e => e.ControllerName).AsQueryable();

            var filteredCount = query.Count();

            query = query.Sorting(requestModel);
            query = query.Paging(requestModel);
            var list = await query.ToListAsync();
            var data = Mapper.Map<List<MenuVm>>(list);

            return new DataTablesResponse(requestModel.Draw, data, filteredCount, totalCount);
        }

        public async Task<IEnumerable<SelectListItem>> GetDropDownAsync(int? selected = 0, int? applicationId = null)
        {
            var query = Context.Menus.OrderBy(e => e.ControllerName).AsQueryable();

            if (applicationId.HasValue)
                query = query.Where(e => e.ApplicationId == applicationId);

            var applications = await query.ToListAsync();

            return applications.Select(e => new SelectListItem
            {
                Text = $"{e.ControllerName} - {e.Application.ApplicationName}",
                Value = e.MenuId.ToString(),
                Selected = e.MenuId == selected
            });
        }

        public async Task<IList<MenuGroupByVm>> GetMenuByApplication(int? appid, int? roleId)
        {
            var menus = await MenuByApplication(appid).ToListAsync();
            var roleMenus = await MenuByRole(roleId).ToListAsync();

            if (roleMenus.Any())
                menus = menus.Select(mg =>
                {
                    mg.Menus = mg.Menus.Select(l =>
                    {
                        l.IsAssigned = roleMenus.Select(e => e.MenuId).Contains(l.MenuId);
                        return l;
                    }).ToList();

                    return mg;
                }).ToList();

            return menus;
        }

        private IQueryable<MenuGroupByVm> MenuByApplication(int? appid)
        {
            var menuGroups = Context.Applications
                .Include(e => e.Menus)
                .Where(e => e.ApplicationId == appid)
                .GroupBy(e => new { e.ApplicationId, e.ApplicationName })
                .Select(g => new MenuGroupByVm
                {
                    ApplicationId = g.Key.ApplicationId,
                    ApplicationName = g.Key.ApplicationName,
                    Menus = g.SelectMany(m => m.Menus.Where(e => e.IsPublished == true).Select(l => new MenuAssignVm
                    {
                        MenuId = l.MenuId,
                        Title = l.Title,
                        ControllerName = l.ControllerName,
                        ApplicationId = l.ApplicationId
                    })).ToList()
                }).OrderBy(e => e.ApplicationName);

            return menuGroups;
        }

        private IQueryable<RolePrivilege> MenuByRole(int? roleId)
        {
            return Context.RolePrivileges.Where(e => e.RoleId == roleId);
        }

        /// <summary>
        /// Get menu list with user assign information
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns></returns>
        public async Task<IList<MenuGroupByVm>> GetByUserIdAsync(int userId)
        {
            var menus = await MenuByApplication().ToListAsync();
            var userMenus = await UserMenuByUserId(userId).ToListAsync();

            if (userMenus.Any())
                menus = menus.Select(mg =>
                {
                    mg.Menus = mg.Menus.Select(l =>
                    {
                        l.IsAssigned = userMenus.Select(e => e.MenuId).Contains(l.MenuId);
                        return l;
                    }).ToList();

                    return mg;
                }).ToList();

            return menus;
        }
    }
}