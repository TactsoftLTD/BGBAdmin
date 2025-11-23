using System.Collections.Generic;

namespace IDIMAdmin.Models.User
{
    public class UserDetailVm
    {
        public UserDetailVm()
        {
            User = new UserVm();
            Applications = new List<ApplicationVm>();
            Menus = new List<MenuGroupByVm>();
            Devices = new List<DeviceVm>();
        }

        public UserVm User { get; set; }
        public IList<ApplicationVm> Applications { get; set; }
        public IList<MenuGroupByVm> Menus { get; set; }
        public IList<DeviceVm> Devices { get; set; }

    }
}