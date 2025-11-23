using FluentValidation;
using IDIMAdmin.Models.User;

namespace IDIMAdmin.Models.Validation.User
{
    public class UserRegimentVmValidator : AbstractValidator<UserRegimentVm>
    {
        public UserRegimentVmValidator()
        {
            RuleFor(m => m.ArmyId)
                .NotEmpty()
                .WithMessage("Select Regiment");

            RuleFor(m => m.UserId)
                .NotEmpty()
                .WithMessage("Select User");
        }
    }
}