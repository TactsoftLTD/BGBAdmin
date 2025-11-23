using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace IDIMAdmin.Models.Admin
{
	public class ActivityLogSearchVm
    {
        public ActivityLogSearchVm()
        {
            ApplicationDropdown = new List<SelectListItem>();
            ActivityLog = new List<ActivityLogVm>();
        }

        public int? UserId { get; set; }
        [DisplayName("Application")]
        public int? ApplicationId { get; set; }
        [DisplayName("User Name")]
        public string UserName { get; set; }
        public string SearchValues { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public IEnumerable<SelectListItem> ApplicationDropdown { get; set; }
        public IList<ActivityLogVm> ActivityLog { get; set; }
    }
}