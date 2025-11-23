using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IDIMAdmin.Models.User
{
    public class UserSearchVm
    {
        public UserSearchVm()
        {
            Users = new List<UserVm>();
            ApplicationDropdown = new List<SelectListItem>();
        }

        public string Username { get; set; }
        public int? ArmyId { get; set; }
        public string RegimentNo { get; set; }
        [Display(Name = "Application")]
        public int? ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        [DisplayName("Active")]
        public Active Active { get; set; }
        public IList<UserVm> Users { get; set; }
        public IEnumerable<SelectListItem> ApplicationDropdown { get; set; }
        
    }

    public enum Active
    {
        All,
        Active,

        [Display(Name = "Not Active")]
        NotActive
    }
}