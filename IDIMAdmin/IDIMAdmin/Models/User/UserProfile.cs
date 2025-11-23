using System.Collections.Generic;

using IDIMAdmin.Models.Setup;

namespace IDIMAdmin.Models.User
{
	public class UserProfile
    {
        public UserVm User { get; set; }
        public GeneralInformationVm  Regiment { get; set; }
        public IList<MenuGroupByVm> Menus { get; set; }
        public IList<ApplicationVm> Applications { get; set; }
        public IList<DeviceVm> Devices { get; set; }
    }
}