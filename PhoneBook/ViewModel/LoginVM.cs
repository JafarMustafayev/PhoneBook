namespace PhoneBook.ViewModel;

public class LoginVM
{
    public string? EmailOrUsername { get; set; }

    public string? Password { get; set; }

    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}