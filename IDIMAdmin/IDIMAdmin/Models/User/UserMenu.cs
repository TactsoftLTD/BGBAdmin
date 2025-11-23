using FluentValidation.Attributes;

using IDIMAdmin.Models.Validation.User;

namespace IDIMAdmin.Models.User
{
	[Validator(typeof(UserMenuVmValidator))]
    public class UserMenu
    {
        public UserMenu()
        {
            User = new UserVm();
            Menu = new MenuVm();
        }

        public int? UserId { get; set; }
        public string UserName { get; set; }
        public int? ArmyId { get; set; }
        public string RegimentNo { get; set; }

        public UserVm User { get; set; }
        public MenuVm Menu { get; set; }
    }
}