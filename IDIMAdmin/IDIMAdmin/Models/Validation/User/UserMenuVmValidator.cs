using FluentValidation;
using IDIMAdmin.Models.User;

namespace IDIMAdmin.Models.Validation.User
{
    public class UserMenuVmValidator : AbstractValidator<UserRegimentVm>
    {
        public UserMenuVmValidator()
        {
            RuleFor(m => m.ArmyId)
                .NotEmpty()
                .WithMessage("Select Regiment");
        }
    }
}