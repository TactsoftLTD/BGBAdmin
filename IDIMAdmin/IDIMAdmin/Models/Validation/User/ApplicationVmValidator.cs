using FluentValidation;
using IDIMAdmin.Models.User;

namespace IDIMAdmin.Models.Validation.User
{
    public class ApplicationVmValidator: AbstractValidator<ApplicationVm>
    {
        public ApplicationVmValidator()
        {
            RuleFor(e => e.ApplicationName)
                .NotEmpty();
            RuleFor(e => e.ApplicationCode)
                .NotEmpty();
            RuleFor(e => e.ApplicationShortName)
                .NotEmpty();
            RuleFor(e => e.Url)
                .NotEmpty();
        }
    }
}