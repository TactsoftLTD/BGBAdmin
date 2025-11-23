using FluentValidation;

namespace IDIMAdmin.Models.User
{
	public class ImageSlideVmValidator : AbstractValidator<ImageSlideVm>
    {
        public ImageSlideVmValidator()
        {
            RuleFor(e => e.Title)
               .NotEmpty();

            RuleFor(e => e.AlternateText)
                .NotEmpty();

            RuleFor(e => e.Priority)
               .NotEmpty();
        }
    }
}