namespace PhoneBook.Validators;

public class LoginVMValidator : AbstractValidator<LoginVM>
{
    public LoginVMValidator()
    {
        RuleFor(x => x.EmailOrUsername)
            .NotNull().WithMessage("Email or Username is required")
            .NotEmpty().WithMessage("Email or Username is required")
            .MaximumLength(64).MinimumLength(3).WithMessage("The input must be between 5 and 64 characters in length.");

        RuleFor(x => x.Password)
            .NotNull().WithMessage("Password is required")
            .NotEmpty().WithMessage("Password is required")
            .MaximumLength(64).MinimumLength(8).WithMessage("The input must be between 5 and 64 characters in length.");
    }
}