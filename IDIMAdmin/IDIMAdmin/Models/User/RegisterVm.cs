using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using IDIMAdmin.Models.Validation.User;

namespace IDIMAdmin.Models.User
{
    [Validator(typeof(RegisterVmValidator))]
    public class RegisterVm
    {
        [DisplayName("Id")]
        public int UserId { get; set; }

        [DisplayName("Regiment No.")]
        public int? ArmyId { get; set; }
        [DisplayName("Regiment No.")]
        public string RegimentNo { get; set; }
        public string Username { get; set; }


        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "The password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number.")]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        public string RePassword { get; set; }

        [DisplayName("Resource Type")]
        public int ResourceType { get; set; }

        [DisplayName("User Type")]
        public int UserType { get; set; }

        [DisplayName("Personnel Code")]
        public string PersonnelCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; } = true;
        [DisplayName("Is All")]
        public bool IsAll { get; set; } 

        public int CreatedUser { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedUser { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int UpdateNo { get; set; }

        // For selected Unite IDs
        [DisplayName("Select Multiple Units")]
        public List<int> SelectedUniteIds { get; set; } = new List<int>();


        // For dropdown
        public IEnumerable<SelectListItem> UnitList { get; set; }

        public IEnumerable<SelectListItem> UserTypeList { get; set; }

        //public virtual ICollection<UserApplication> UserApplications { get; set; }
        //public virtual ICollection<UserDevice> UserDevices { get; set; }
        //public virtual ICollection<UserLoginDevice> UserLoginDevices { get; set; }
        //public virtual ICollection<UserPriviledge> UserPriviledges { get; set; }

    }
}