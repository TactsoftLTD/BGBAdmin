using FluentValidation;
using IDIMAdmin.Models.User;

namespace IDIMAdmin.Models.Validation.User
{
    public class RoleVmValidator : AbstractValidator<RoleVm>
    {
        public RoleVmValidator()
        {
            RuleFor(e => e.ApplicationId)
                .NotEmpty();

            RuleFor(e => e.Name)
                .NotEmpty();
        }
    }
}