using FluentValidation.Attributes;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IDIMAdmin.Models.User
{
	[Validator(typeof(UserChangePasswordVmValidator))]
    public class UserChangePasswordVm
    {
        public string Username { get; set; }

        [DisplayName("Current Password")]
        public string Password { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "The password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number.")]
        [DisplayName("New Password")]
        public string NewPassword { get; set; }

        [DisplayName("Confirm New Password")]
        public string ReNewPassword { get; set; }
    }
}