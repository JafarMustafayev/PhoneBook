namespace PhoneBook.Service;

public interface IMailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}