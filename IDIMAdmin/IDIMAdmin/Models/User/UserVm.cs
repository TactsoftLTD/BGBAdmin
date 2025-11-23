using FluentValidation.Attributes;
using IDIMAdmin.Models.Validation.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace IDIMAdmin.Models.User
{
    [Validator(typeof(UserVmValidator))]
    public class UserVm
    {
        [DisplayName("Id")]
        public int UserId { get; set; }

        [DisplayName("Regiment No.")]
        public int? ArmyId { get; set; }
        [DisplayName("Regiment No.")]
        public string RegimentNo { get; set; }

        public string Username { get; set; }

        public int UserType { get; set; }   

        public string Password { get; set; }

        [DisplayName("Personnel Code")]
        public string PersonnelCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public string UniteList { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
        [DisplayName("Is All")]
        public bool IsAll { get; set; }

        public int CreatedUser { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedUser { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int UpdateNo { get; set; }


        
    }
}