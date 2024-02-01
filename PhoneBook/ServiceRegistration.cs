using RIDO.Services;

namespace PhoneBook;

public static class ServiceRegistration
{
    public static void AddService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews();

        services.AddFluentValidation(v =>
       {
           v.RegisterValidatorsFromAssemblyContaining<PersonCreateValidator>();
       });
        services.AddAutoMapper(typeof(PersonProfile));

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Default"));
        });

        services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
        services.AddTransient<IMailService, MailService>();

        services.AddIdentity<AppUser, IdentityRole>(opt =>
        {
            opt.Password.RequiredLength = 8;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequireDigit = false;
            opt.Password.RequireLowercase = true;
            opt.Password.RequireUppercase = true;
            opt.User.RequireUniqueEmail = true;
            opt.SignIn.RequireConfirmedEmail = false;
        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
    }
}