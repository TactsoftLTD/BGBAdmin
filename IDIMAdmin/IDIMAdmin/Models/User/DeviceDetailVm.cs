using System.Collections.Generic;

namespace IDIMAdmin.Models.User
{
	public class DeviceDetailVm
    {
        public DeviceDetailVm()
        {
            Device = new DeviceVm();
            Users = new List<UserVm>();
        }

        public DeviceVm Device { get; set; }
        public IList<UserVm> Users { get; set; }
    }
}