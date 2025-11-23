using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IDIMAdmin.Models.User
{
    public class UserMenuAssignVm
    {
        public UserMenuAssignVm()
        {
            Menus = new List<MenuGroupByVm>();
        }

        [Display(Name = "Id")]
        public int UserId { get; set; }

        [Display(Name = "Regiment No.")]
        public string RegimentNo { get; set; }

        public string Username { get; set; }

        public IList<MenuGroupByVm> Menus { get; set; }
    }
}