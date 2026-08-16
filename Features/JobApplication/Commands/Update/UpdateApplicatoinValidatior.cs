using FluentValidation;

public class UpdateApplicationValidator : AbstractValidator<UpdateApplicationCommand>
{
    public UpdateApplicationValidator()
    {
        RuleFor(x => x.Position)
            .NotEmpty()
            .When(x => x.Position != null)
            .WithMessage("Position cannot be empty if provided.");

        RuleFor(x => x.Status)
            .IsEnumName(typeof(ApplicationStatus), caseSensitive: false)
            .When(x => !string.IsNullOrEmpty(x.Status))
            .WithMessage("Invalid application status.");
    }
}