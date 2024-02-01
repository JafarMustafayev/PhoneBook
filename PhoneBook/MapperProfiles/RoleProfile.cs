namespace PhoneBook.MapperProfiles;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<IdentityRole, RoleVM>().ReverseMap();
    }
}