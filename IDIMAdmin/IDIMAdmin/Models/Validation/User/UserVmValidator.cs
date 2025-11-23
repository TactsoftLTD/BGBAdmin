using FluentValidation;
using IDIMAdmin.Models.User;

namespace IDIMAdmin.Models.Validation.User
{
    public class UserVmValidator : AbstractValidator<UserVm>
    {
        public UserVmValidator()
        {
            RuleFor(e => e.Username)
                .NotEmpty();
            RuleFor(e => e.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}