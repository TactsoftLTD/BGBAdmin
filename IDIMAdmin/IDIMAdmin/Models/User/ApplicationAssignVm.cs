using System.ComponentModel.DataAnnotations;

namespace IDIMAdmin.Models.User
{
    public class ApplicationAssignVm
    {
        public ApplicationAssignVm()
        {
            Application = new ApplicationVm();
        }

        [Display(Name = "Is Assigned")]
        public bool IsAssigned { get; set; }

        public ApplicationVm Application { get; set; }
    }
}