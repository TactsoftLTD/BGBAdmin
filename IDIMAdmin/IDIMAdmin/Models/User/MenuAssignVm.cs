using System.ComponentModel.DataAnnotations;

namespace IDIMAdmin.Models.User
{
    public class MenuAssignVm
    {
        [Display(Name = "Is Assigned")]
        public bool IsAssigned { get; set; }

        public int MenuId { get; set; }
        public string Title { get; set; }
        public string ControllerName { get; set; }
        public int ApplicationId { get; set; }
    }
}