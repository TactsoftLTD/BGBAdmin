using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IDIMAdmin.Models.User
{
	public class MenuSearchVm
    {
        public MenuSearchVm()
        {
            ApplicationDropdown = new List<SelectListItem>();
            Menus = new List<MenuVm>();
        }

        public string Title { get; set; }

        [Display(Name = "Application")]
        public int? ApplicationId { get; set; }

        [Display(Name = "Menu")]
        public string ControllerName { get; set; }
        
        public IEnumerable<SelectListItem> ApplicationDropdown { get; set; }
        public IList<MenuVm> Menus { get; set; }
    }
}