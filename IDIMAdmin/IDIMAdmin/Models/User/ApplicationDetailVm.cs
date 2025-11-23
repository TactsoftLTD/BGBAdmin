using System.Collections.Generic;

namespace IDIMAdmin.Models.User
{
    public class ApplicationDetailVm
    {
        public ApplicationDetailVm()
        {
            Users= new List<UserVm>();
            Menus = new List<MenuVm>();
            Devices = new List<DeviceVm>();
        }

        public ApplicationVm Application { get; set; }
        public IList<UserVm> Users { get; set; }
        public IList<MenuVm> Menus { get; set; }
        public IList<DeviceVm> Devices { get; set; }
    }
}