using FluentValidation;

namespace IsoTreatmentProcessSupportAPI.Models.Validators
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty();

            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty();

            RuleFor(x => x.ConfirmNewPassword).Equal(e => e.NewPassword).WithMessage("Passwords must match");
        }
    }
}
