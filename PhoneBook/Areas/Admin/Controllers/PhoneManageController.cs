namespace PhoneBook.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class PhoneManageController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _evn;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public PhoneManageController(AppDbContext context,
                                 IWebHostEnvironment evn,
                                 IMapper mapper,
                                 UserManager<AppUser> userManager)
    {
        _context = context;
        _evn = evn;
        _mapper = mapper;
        _userManager = userManager;
    }

    #region INDEX

    public async Task<IActionResult> Index(string? searchQuery)
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);

        var role = await _userManager.GetRolesAsync(user);

        var phonesQuery = _context.Persons
            .OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ThenBy(x => x.FatherName)
            .AsQueryable();

        if (role != null && !role.Contains("SuperAdmin"))
        {
            phonesQuery = phonesQuery.Where(x => x.IsDeleted == false);
        }

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

    #endregion INDEX

    #region CREATE

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PersonCreateVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var personDb = _context.Persons.FirstOrDefault(x => x.PhoneNumber == model.PhoneNumber);
        if (personDb != null)
        {
            ModelState.AddModelError("PhoneNumber", "Bu nomre evvelceden daxil olunub");
            return View(model);
        }
        personDb = _context.Persons.FirstOrDefault(x => x.InternalNumber == model.InternalNumber);
        if (personDb != null)
        {
            ModelState.AddModelError("InternalNumber", "Bu daxili nomre evvelceden daxil olunub");
            return View(model);
        }

        if (model.Email != null)
        {
            personDb = _context.Persons.FirstOrDefault(x => x.Email == model.Email);
            if (personDb != null)
            {
                ModelState.AddModelError("Email", "Bu email evvelceden daxil olunub");
                return View(model);
            }
        }

        string fileName = null;
        if (model.ImageFile != null)
        {
            if (!model.ImageFile.ContentType.Contains("image"))
            {
                ModelState.AddModelError("ImageFile", "Fayl png ve ya jpeg formatinda olmalidir");
                return View(model);
            }

            if (model.ImageFile.Length > (1024 * 1024) * 5)
            {
                ModelState.AddModelError("ImageFile", "Fayl olcusu 5mb-dan cox olmamalidir");
                return View(model);
            }

            string uploadFolder = Path.Combine(_evn.WebRootPath, "Uploads", "PersonImages");
            fileName = await FileService.SaveFile(uploadFolder, model.ImageFile);
        }
        else
        {
            fileName = "default.jpg";
        }

        var person = _mapper.Map<Person>(model);
        person.Image = fileName;

        await _context.Persons.AddAsync(person);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    #endregion CREATE

    #region UPDATE

    public async Task<IActionResult> Update(string Id)
    {
        var person = _context.Persons.FirstOrDefault(x => x.Id == Id);
        if (person == null)
        {
            return NotFound();
        }

        var map = _mapper.Map<PersonUpdateVM>(person);

        var user = await _userManager.FindByNameAsync(User.Identity.Name);

        var role = await _userManager.GetRolesAsync(user);

        if (role != null && role.Contains("SuperAdmin"))
        {
            return View("UpdateForSA", map);
        }

        return View(map);
    }

    [HttpPost]
    public async Task<IActionResult> Update(PersonUpdateVM model)
    {
        Person person = _context.Persons.FirstOrDefault(x => x.Id == model.Id);
        if (person == null)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            model.ImagePath = person.Image;
            return View(model);
        }

        var personDb = _context.Persons.FirstOrDefault(x => x.PhoneNumber == model.PhoneNumber && x.Id != model.Id);

        if (personDb != null)
        {
            ModelState.AddModelError("PhoneNumber", "Bu nomre evvelceden daxil olunub");
            return View(model);
        }

        personDb = _context.Persons.FirstOrDefault(x => x.InternalNumber == model.InternalNumber && x.Id != model.Id);
        if (personDb != null)
        {
            ModelState.AddModelError("InternalNumber", "Bu daxili nomre evvelceden daxil olunub");
            return View(model);
        }

        if (model.Email != null)
        {
            personDb = _context.Persons.FirstOrDefault(x => x.Email == model.Email && x.Id != model.Id);
            if (personDb != null)
            {
                ModelState.AddModelError("Email", "Bu email evvelceden daxil olunub");
                return View(model);
            }
        }

        _mapper.Map(model, person);

        if (model.DeletedImage)
        {
            if (person.Image != "default.jpg")
            {
                FileService.DeleteFile(Path.Combine(_evn.WebRootPath, "Uploads", "PersonImages", person.Image));
            }
            person.Image = "default.jpg";
        }
        if (model.ImageFile != null)
        {
            if (!model.ImageFile.ContentType.Contains("image"))
            {
                if (person.Image != "default.jpg")
                {
                    FileService.DeleteFile(Path.Combine(_evn.WebRootPath, "Uploads", "PersonImages", person.Image));
                }
                return View(model);
            }

            if (model.ImageFile.Length > (1024 * 1024) * 5)
            {
                ModelState.AddModelError("ImageFile", "Fayl olcusu 5mb-dan cox olmamalidir");
                return View(model);
            }

            if (person.Image != "default.jpg")
            {
                FileService.DeleteFile(Path.Combine(_evn.WebRootPath, "Uploads", "PersonImages", person.Image));
            }

            string uploadFolder = Path.Combine(_evn.WebRootPath, "Uploads", "PersonImages");
            string fileName = FileService.SaveFile(uploadFolder, model.ImageFile).Result;
            person.Image = fileName;
        }
        else
        {
            person.Image = person.Image;
        }

        _context.Persons.Update(person);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateForSA(PersonUpdateVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        Person person = _context.Persons.FirstOrDefault(x => x.Id == model.Id);

        if (person == null)
        {
            return NotFound();
        }

        var personDb = _context.Persons.FirstOrDefault(x => x.PhoneNumber == model.PhoneNumber && x.Id != model.Id);

        if (personDb != null)
        {
            ModelState.AddModelError("PhoneNumber", "Bu nomre evvelceden daxil olunub");
            return View(model);
        }

        personDb = _context.Persons.FirstOrDefault(x => x.InternalNumber == model.InternalNumber && x.Id != model.Id);
        if (personDb != null)
        {
            ModelState.AddModelError("InternalNumber", "Bu daxili nomre evvelceden daxil olunub");
            return View(model);
        }

        if (model.Email != null)
        {
            personDb = _context.Persons.FirstOrDefault(x => x.Email == model.Email && x.Id != model.Id);
            if (personDb != null)
            {
                ModelState.AddModelError("Email", "Bu email evvelceden daxil olunub");
                return View(model);
            }
        }

        _mapper.Map(model, person);

        if (model.DeletedImage)
        {
            if (person.Image != "default.jpg")
            {
                FileService.DeleteFile(Path.Combine(_evn.WebRootPath, "Uploads", "PersonImages", person.Image));
            }
            person.Image = "default.jpg";
        }
        if (model.ImageFile != null)
        {
            if (!model.ImageFile.ContentType.Contains("image"))
            {
                ModelState.AddModelError("ImageFile", "Fayl png ve ya jpeg formatinda olmalidir");
                return View(model);
            }

            if (model.ImageFile.Length > (1024 * 1024) * 5)
            {
                ModelState.AddModelError("ImageFile", "Fayl olcusu 5mb-dan cox olmamalidir");
                return View(model);
            }

            if (person.Image != "default.jpg")
            {
                FileService.DeleteFile(Path.Combine(_evn.WebRootPath, "Uploads", "PersonImages", person.Image));
            }

            string uploadFolder = Path.Combine(_evn.WebRootPath, "Uploads", "PersonImages");
            string fileName = FileService.SaveFile(uploadFolder, model.ImageFile).Result;
            person.Image = fileName;
        }
        else
        {
            person.Image = person.Image;
        }

        _context.Persons.Update(person);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    #endregion UPDATE

    #region DELETE

    [HttpDelete]
    public async Task<IActionResult> Delete(string Id)
    {
        var person = _context.Persons.FirstOrDefault(x => x.Id == Id);
        if (person == null)
        {
            return Ok(new ResponseDTO { IsSuccess = false, Message = "Şəxs silinə bilmədi" });
        }

        person.IsDeleted = true;
        _context.Persons.Update(person);
        var res = await _context.SaveChangesAsync();

        if (res == 1)
            return Ok(new ResponseDTO { IsSuccess = true, Message = "Şəxs uğurla silindi" });
        else
            return Ok(new ResponseDTO { IsSuccess = false, Message = "Şəxs silinə bilmədi" });
    }

    [HttpDelete]
    public async Task<IActionResult> HardDelete(string Id)
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);

        var role = await _userManager.GetRolesAsync(user);

        if (role != null && role.Contains("SuperAdmin"))
        {
            var person = _context.Persons.FirstOrDefault(x => x.Id == Id);
            if (person == null)
            {
                return Ok(new ResponseDTO { IsSuccess = false, Message = "Şəxs silinə bilmədi" });
            }

            if (person.Image != "default.jpg")
            {
                FileService.DeleteFile(Path.Combine(_evn.WebRootPath, "Uploads", "PersonImages", person.Image));
            }

            _context.Persons.Remove(person);
            var res = await _context.SaveChangesAsync();

            if (res == 1)
                return Ok(new ResponseDTO { IsSuccess = true, Message = "Şəxs uğurla silindi" });
            else
                return Ok(new ResponseDTO { IsSuccess = false, Message = "Şəxs silinə bilmədi" });
        }
        else
        {
            return Ok(new ResponseDTO { IsSuccess = false, Message = "istifadəçini silmək icazəniz yoxdur" });
        }
    }

    #endregion DELETE
}