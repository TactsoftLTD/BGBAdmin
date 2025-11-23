using FluentValidation.Attributes;

using IDIMAdmin.Models.Setup;
using IDIMAdmin.Models.Validation.User;

namespace IDIMAdmin.Models.User
{
	[Validator(typeof(UserRegimentVmValidator))]
    public class UserRegimentVm
    {
        public UserRegimentVm()
        {
            User = new UserVm();
            Regiment = new GeneralInformationVm();
        }

        public int? UserId { get; set; }
        public string UserName { get; set; }
        public int? ArmyId { get; set; }
        public string RegimentNo { get; set; }

        public UserVm User { get; set; }
        public GeneralInformationVm Regiment { get; set; }
    }
}