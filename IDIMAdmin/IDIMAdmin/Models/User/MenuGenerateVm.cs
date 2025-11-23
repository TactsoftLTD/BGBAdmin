using System.Collections.Generic;

namespace IDIMAdmin.Models.User
{
	public class MenuGenerateVm
    {
        public MenuGenerateVm()
        {
            Menus = new List<MenuVm>();
        }

        public IList<MenuVm> Menus { get; set; }
    }
}