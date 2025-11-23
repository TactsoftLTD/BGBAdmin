using System.Collections.Generic;

namespace IDIMAdmin.Models.User
{
    public class MenuGroupByVm
    {
        public MenuGroupByVm()
        {
            Menus = new List<MenuAssignVm>();
        }
        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public IList<MenuAssignVm> Menus { get; set; }
    }
}