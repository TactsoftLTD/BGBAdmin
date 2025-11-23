using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace IDIMAdmin.Models.User
{
    public class UserApplicationVm
    {
        [DisplayName("Id")]
        public int UserApplicationId { get; set; }

        [DisplayName("User")]
        public int UserId { get; set; }

        [DisplayName("Application")]
        public int ApplicationId { get; set; }

        public int CreatedUser { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedUser { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int UpdateNo { get; set; }

        //public virtual ApplicationVm Application { get; set; }
        //public virtual UserVm User { get; set; }

        public IEnumerable<SelectListItem> UserDropdown { get; set; }
        public IEnumerable<SelectListItem> ApplicationDropdown { get; set; }
    }
}