namespace PhoneBook.MapperProfiles;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<Person, PersonHomeVM>()
            .ForMember(
            dest => dest.ImagePath,
            opt => opt.MapFrom(src => src.Image)).ReverseMap();
        CreateMap<Person, PersonCreateVM>().ReverseMap();
        CreateMap<Person, PersonUpdateVM>().
            ForMember(
            dest => dest.ImagePath,
            opt => opt.MapFrom(src => src.Image));
        CreateMap<PersonUpdateVM, Person>();
    }
}