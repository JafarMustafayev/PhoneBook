namespace PhoneBook.Models;

public class BaseEntity
{
    public string Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsDeleted { get; set; }

    public BaseEntity()
    {
        Id = Guid.NewGuid().ToString();
        CreatedDate = DateTime.Now;
        IsDeleted = false;
    }
}