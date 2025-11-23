using FluentValidation;
using IDIMAdmin.Models.User;

namespace IDIMAdmin.Models.Validation.User
{
    public class RegisterVmValidator : AbstractValidator<RegisterVm>
    {
        public RegisterVmValidator()
        {
            RuleFor(e => e.Username)
                .NotEmpty();

            RuleFor(e => e.Password)
                .NotEmpty();

            RuleFor(e => e.RePassword)
                .Equal(m => m.Password)
                .WithMessage("Password does not match");

            RuleFor(e => e.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}