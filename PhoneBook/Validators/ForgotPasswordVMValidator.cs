namespace PhoneBook.Validators;

public class ForgotPasswordVMValidator : AbstractValidator<ForgotPasswordVM>
{
    public ForgotPasswordVMValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .NotNull().WithMessage("Email is required")
            .MaximumLength(64).MinimumLength(5).WithMessage("The input must be between 5 and 64 characters in length.")
            .EmailAddress();
    }
}