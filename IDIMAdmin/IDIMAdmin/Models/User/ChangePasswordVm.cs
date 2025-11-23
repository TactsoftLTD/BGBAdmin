using System.ComponentModel.DataAnnotations;

namespace IDIMAdmin.Models.User
{
	public class ChangePasswordVm
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "The password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number.")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm New Password")]
        public string ReNewPassword { get; set; }
    }
}