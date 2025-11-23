using FluentValidation.Attributes;
using IDIMAdmin.Models.Validation.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IDIMAdmin.Models.User
{
    [Validator(typeof(MenuVmValidator))]
    public class MenuVm
    {
        public MenuVm()
        {
            ApplicationDropdown = new List<SelectListItem>();
        }

        public int MenuId { get; set; }
        public string Title { get; set; }

        [Display(Name = "Application")]
        public int ApplicationId { get; set; }

        [Display(Name = "Application")]
        public string ApplicationName { get; set; }

        [Display(Name = "Menu")]
        public string ControllerName { get; set; }

        [Display(Name = "Type")]
        public MenuType MenuType { get; set; }
        public int Priority { get; set; }

        public string Icon { get; set; } = "check";

        [Display(Name = "Publish")]
        public bool IsPublished { get; set; } = true;
        public string Remark { get; set; }
        public int CreatedUser { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedUser { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int UpdateNo { get; set; }
        public IEnumerable<SelectListItem> ApplicationDropdown { get; set; }
    }
}