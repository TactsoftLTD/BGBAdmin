using System.Collections.Generic;
using System.Linq;

using IDIMAdmin.Entity;
using IDIMAdmin.Models.User;

namespace IDIMAdmin.Extentions
{
	public static class MenuModel
    {
        public static IList<Menu> Admin()
        {
            var menus = new List<Menu>
            {
                new Menu{ControllerName = "Dashboard", Title = "Dashboard", MenuType = (int)MenuType.Main, Priority = 1, Icon = "dashboard"},
                new Menu{ControllerName = "Home", Title = "Home", MenuType = (int)MenuType.Main, Priority = 2, Icon = "home"},
                new Menu{ControllerName = "Report", Title = "Report", MenuType = (int)MenuType.Main, Priority = 3, Icon = "line-chart"},

                new Menu{ControllerName = "UserRegiment", Title = "User Regiment", MenuType = (int)MenuType.Information, Priority = 1, Icon = "plus"},
                new Menu{ControllerName = "UserApplication", Title = "User Application", MenuType = (int)MenuType.Information, Priority = 2, Icon = "plus"},
                new Menu{ControllerName = "UserMenu", Title = "User Menu", MenuType = (int)MenuType.Information, Priority = 3, Icon = "plus"},

                new Menu{ControllerName = "Application", Title = "Application", MenuType = (int)MenuType.Setup, Priority = 1, Icon = "building"},
                new Menu{ControllerName = "Menu", Title = "Menu", MenuType = (int)MenuType.Setup, Priority = 2, Icon = "bars"},
                new Menu{ControllerName = "Device", Title = "Device", MenuType = (int)MenuType.Setup, Priority = 3, Icon = "desktop"},
                new Menu{ControllerName = "User", Title = "User", MenuType = (int)MenuType.Setup, Priority = 4, Icon = "users"},

                new Menu{ControllerName = "GeneralInformation", Title = "Dashboard", MenuType = (int)MenuType.Other, Priority = 1, Icon = "dashboard"},
                new Menu{ControllerName = "Unit", Title = "Dashboard", MenuType = (int)MenuType.Other, Priority = 2, Icon = "dashboard"}
            };

            return menus.Select(m =>
            {
                m.ControllerName = m.ControllerName.ToLower();
                m.ApplicationId = DefaultData.ApplicationId;
                m.IsPublished = true;
                return m;
            }).ToList();
        }
    }
}