namespace PhoneBook.Validators;

public class UserCreateVMValidator : AbstractValidator<UserCreateVM>
{
    public UserCreateVMValidator()
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

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
             .NotNull().WithMessage("Password is required")
            .MaximumLength(64).MinimumLength(8).WithMessage("The input must be between 8 and 64 characters in length.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Password is required")
            .NotNull().WithMessage("Password is required")
            .MaximumLength(64).MinimumLength(8).WithMessage("The input must be between 8 and 64 characters in length.")
            .Equal(x => x.Password).WithMessage("Confirm Password must be equal to Password");

        RuleFor(x => x.FullName)
            .MaximumLength(64).MinimumLength(4).WithMessage("The input must be between 4 and 64 characters in length.");
    }
}