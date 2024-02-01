namespace PhoneBook.Validators;

public class UserUpdateVMValidator : AbstractValidator<UserUpdateVM>
{
    public UserUpdateVMValidator()
    {
        RuleFor(x => x.Username)
           .NotEmpty().WithMessage("Username is required")
           .NotNull().WithMessage("Username is required")
           .MaximumLength(64).MinimumLength(4).WithMessage("The input must be between 4 and 64 characters in length.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .NotNull().WithMessage("Email is required")
            .MaximumLength(64).MinimumLength(5).WithMessage("The input must be between 5 and 64 characters in length.")
            .EmailAddress();

        RuleFor(x => x.FullName)
            .MaximumLength(64).MinimumLength(4).WithMessage("The input must be between 4 and 64 characters in length.");
    }
}