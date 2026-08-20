using FluentValidation;

public class CreateApplicationValidator : AbstractValidator<CreateApplicationCommand>
{
    public CreateApplicationValidator()
    {
        RuleFor(x => x.CompanyId).GreaterThan(0).WithMessage("CompanyId must be greater than 0.");
        RuleFor(x => x.UserId).GreaterThan(0).WithMessage("UserId must be greater than 0.");
        RuleFor(x => x.Position).NotEmpty().WithMessage("Position is required.");
    }
}