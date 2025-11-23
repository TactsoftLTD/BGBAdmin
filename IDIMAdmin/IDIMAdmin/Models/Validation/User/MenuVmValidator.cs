using FluentValidation;
using IDIMAdmin.Models.User;

namespace IDIMAdmin.Models.Validation.User
{
    public class MenuVmValidator : AbstractValidator<MenuVm>
    {
        public MenuVmValidator()
        {
            RuleFor(e => e.ApplicationId)
                .NotEmpty();

            RuleFor(e => e.Title)
                .NotEmpty();

            RuleFor(e => e.ControllerName)
                .NotEmpty();
        }
    }
}