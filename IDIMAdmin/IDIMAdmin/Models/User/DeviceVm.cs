using System;
using System.ComponentModel.DataAnnotations;

namespace IDIMAdmin.Models.User
{
	public class DeviceVm
    {
        [Display(Name="Id")]
        public int DeviceId { get; set; }
        [Display(Name = "Device")]
        public string DeviceName { get; set; }

        public int CreatedUser { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedUser { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int UpdateNo { get; set; }
    }
}