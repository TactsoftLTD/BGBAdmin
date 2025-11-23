using FluentValidation.Attributes;
using IDIMAdmin.Models.Validation.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IDIMAdmin.Models.User
{
    [Validator(typeof(RoleVmValidator))]
    public class RoleVm
    {
        public RoleVm()
        {
            ApplicationDropdown = new List<SelectListItem>();
        }

        public int RoleId { get; set; }
        [Display(Name = "Role Name")]
        public string Name { get; set; }
        public string ApplicationName { get; set; }
        [Display(Name = "Application")]
        public int ApplicationId { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
        public int CreatedUser { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedUser { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int UpdateNo { get; set; }

        public IEnumerable<SelectListItem> ApplicationDropdown { get; set; }
    }
}