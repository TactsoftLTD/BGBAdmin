using FluentValidation;

namespace IDIMAdmin.Models.User
{
	public class UserChangePasswordVmValidator : AbstractValidator<UserChangePasswordVm>
    {
        public UserChangePasswordVmValidator()
        {
            RuleFor(m => m.Password)
                 .NotEmpty();

            RuleFor(m => m.NewPassword)
                    .NotEmpty();

            RuleFor(m => m.ReNewPassword)
                    .Equal(customer => customer.NewPassword)
                    .When(customer => !string.IsNullOrWhiteSpace(customer.NewPassword))
                    .WithMessage("Password does not match.");
        }

    }
}