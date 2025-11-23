using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IDIMAdmin.Models.User
{
    public class UserApplicationAssignVm
    {
        [Display(Name = "Id")]
        public int UserId { get; set; }

        [Display(Name = "Regiment No.")]
        public string RegimentNo { get; set; }

        public string Username { get; set; }

        public IList<ApplicationAssignVm> Applications { get; set; }
    }
}