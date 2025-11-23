using FluentValidation;
using IDIMAdmin.Models.Setup;

namespace IDIMAdmin.Models.Validation
{
    public class SetupUnitVmValidator : AbstractValidator<UnitVm>
    {
        public SetupUnitVmValidator()
        {
            RuleFor(e => e.UnitName)
                .NotEmpty();
        }
    }
}