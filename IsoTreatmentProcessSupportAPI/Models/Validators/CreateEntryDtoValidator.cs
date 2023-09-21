using FluentValidation;

namespace IsoTreatmentProcessSupportAPI.Models.Validators
{
    public class CreateEntryDtoValidator : AbstractValidator<CreateEntryDto>
    {
        public CreateEntryDtoValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty();
        }
    }
}
