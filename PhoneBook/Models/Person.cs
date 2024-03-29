﻿namespace PhoneBook.Models;

public class Person : BaseEntity
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? FatherName { get; set; }

    public string? InternalNumber { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Position { get; set; }

    public string? Image { get; set; }
}