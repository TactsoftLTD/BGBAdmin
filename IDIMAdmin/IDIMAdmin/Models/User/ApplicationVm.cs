using System;
using System.Collections.Generic;
using System.ComponentModel;
using FluentValidation.Attributes;
using IDIMAdmin.Models.Validation.User;

namespace IDIMAdmin.Models.User
{
    [Validator(typeof(ApplicationVmValidator))]
    public class ApplicationVm
    {
        [DisplayName("Id")]
        public int ApplicationId { get; set; }

        [DisplayName("Name")]
        public string ApplicationName { get; set; }

        [DisplayName("Short Name")]
        public string ApplicationShortName { get; set; }

        [DisplayName("Code")]
        public string ApplicationCode { get; set; }

        public int Priority { get; set; } = 10;
        public string Url { get; set; } = "#";
        public string Icon { get; set; } = "check";
        public string Color { get; set; } = "A72D32";

        [DisplayName("Status")]
        public bool IsPublished { get; set; } = true;

        public int CreatedUser { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdateUser { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int UpdateNo { get; set; }
        public string IconImagePath { get; set; }

        public virtual ICollection<MenuVm> Menus { get; set; }
    }
}