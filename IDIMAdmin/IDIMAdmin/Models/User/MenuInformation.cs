using System.Collections.Generic;

namespace IDIMAdmin.Models.User
{
    public class MenuInformation
    {
        public MenuInformation()
        {
            Menus = new List<MenuVm>();
        }

        public MenuType MenuType { get; set; }
        public IList<MenuVm> Menus { get; set; }
    }
}