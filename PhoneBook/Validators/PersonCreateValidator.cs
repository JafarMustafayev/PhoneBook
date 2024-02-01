namespace PhoneBook.Validators;

public class PersonCreateValidator : AbstractValidator<PersonCreateVM>
{
    public PersonCreateValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull().WithMessage("Ad bos ola bilmez")
           .NotEmpty().WithMessage("Ad bos ola bilmez")
            .MaximumLength(64).MinimumLength(3).WithMessage("Ad 3 ve 64 simvol arasinda olmalidir");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad bos ola bilmez")
            .MaximumLength(64).MinimumLength(3).WithMessage("Soyad 3 ve 64 simvol arasinda olmalidir");

        RuleFor(x => x.FatherName)
            .NotNull().WithMessage("Ata adi bos ola bilmez")
            .NotEmpty().WithMessage("Ata adi bos ola bilmez")
            .MaximumLength(64).MinimumLength(3).WithMessage("Ata adi 3 ve 64 simvol arasinda olmalidir");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon nomresi bos ola bilmez")
            .NotNull().WithMessage("Telefon nomresi bos ola bilmez")
            .MaximumLength(13).WithMessage("Telefon nomresi 13 simvoldan ibaret olmalidir")
            .Must(BeNumeric).WithMessage("Telefon nomresinin terkibinde yalniz reqem ola biler");

        RuleFor(x => x.InternalNumber)
            .NotEmpty().WithMessage("Daxili nomre bos ola bilmez")
            .NotNull().WithMessage("Daxili nomre bos ola bilmez")
            .MaximumLength(13).WithMessage("Daxili nomre 13 simvoldan ibaret olmalidir")
            .Must(BeNumeric).WithMessage("Daxili nomrenin terkibinde yalniz reqem ola biler");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email adresi duzgun qeyd edilmeyib");

        RuleFor(x => x.Address)
            .NotNull().WithMessage("Unvan bos ola bilmez")
            .NotEmpty().WithMessage("Unvan bos ola bilmez")
            .MaximumLength(64).MinimumLength(3).WithMessage("Unvan 3 ve 64 simvol arasinda olmalidir");

        RuleFor(x => x.Position)
            .NotNull().WithMessage("Vezife bos ola bilmez")
            .NotEmpty().WithMessage("Vezife bos ola bilmez")
            .MaximumLength(64).MinimumLength(3).WithMessage("Vezife 3 ve 64 simvol arasinda olmalidir");
    }

    private bool BeNumeric(string input)
    {
        return int.TryParse(input, out _);
    }
}