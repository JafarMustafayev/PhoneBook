namespace PhoneBook.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.Property(p => p.FirstName)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(p => p.LastName)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(p => p.FatherName)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(p => p.InternalNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.PhoneNumber)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(p => p.Email)
            .HasMaxLength(64)
            .IsRequired(false);

        builder.Property(p => p.Address)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(p => p.Position)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(p => p.Image)
            .HasMaxLength(100)
            .IsRequired();
    }
}