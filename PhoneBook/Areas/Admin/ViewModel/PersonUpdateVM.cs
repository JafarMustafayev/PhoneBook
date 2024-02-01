namespace PhoneBook.Areas.Admin.ViewModel;

public class PersonUpdateVM
{
    public string Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? FatherName { get; set; }

    public string? InternalNumber { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Position { get; set; }

    public string? ImagePath { get; set; }

    public bool IsDeleted { get; set; }

    public bool DeletedImage { get; set; }

    public IFormFile? ImageFile { get; set; }
}