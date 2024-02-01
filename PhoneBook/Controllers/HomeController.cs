using Microsoft.AspNetCore.Mvc;

namespace PhoneBook.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public HomeController(IMapper mapper,
                          AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<IActionResult> Index(string? searchQuery)
    {
        var phonesQuery = _context.Persons.Where(x => x.IsDeleted == false)
            .OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ThenBy(x => x.FatherName)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchQuery))
        {
            var splitQuery = searchQuery?.Split(' ');
            foreach (var query in splitQuery)
            {
                phonesQuery = phonesQuery.Where(x => x.FirstName.Contains(query)
                || x.LastName.Contains(query)
                || x.FatherName.Contains(query)
                || x.PhoneNumber.Contains(query)
                || x.InternalNumber.Contains(query)
                || x.Email.Contains(query)
                || x.Address.Contains(query)
                || x.Position.Contains(query));
            }
        }

        var phones = await phonesQuery.ToListAsync();

        var map = _mapper.Map<List<PersonHomeVM>>(phones);

        if (searchQuery != null)
        {
            return Ok(map);
        }
        return View(map);
    }
}