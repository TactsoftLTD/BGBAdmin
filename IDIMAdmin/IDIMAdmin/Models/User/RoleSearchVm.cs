using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IDIMAdmin.Models.User
{
    public class RoleSearchVm
    {
        public RoleSearchVm()
        {
            ApplicationDropdown = new List<SelectListItem>();
        }

        public string Name { get; set; }

        [Display(Name = "Application")]
        public int? ApplicationId { get; set; }
        public string ApplicationName { get; set; }

        public IEnumerable<SelectListItem> ApplicationDropdown { get; set; }
    }
}