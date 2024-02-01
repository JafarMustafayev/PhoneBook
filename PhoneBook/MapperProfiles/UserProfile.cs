namespace PhoneBook.MapperProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AppUser, SingleUserVM>().ReverseMap();
        CreateMap<UserCreateVM, AppUser>();
    }
}