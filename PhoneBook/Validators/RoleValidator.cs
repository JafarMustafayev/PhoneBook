namespace PhoneBook.Validators;

public class RoleValidator: AbstractValidator<RoleVM>
{
    public RoleValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Role name is required")
            .NotNull().WithMessage("Role name is required")
            .MaximumLength(50).MinimumLength(2);
    }
}
