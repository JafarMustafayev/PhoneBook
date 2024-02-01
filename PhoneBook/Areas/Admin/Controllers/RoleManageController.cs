namespace PhoneBook.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class RoleManageController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;

    public RoleManageController(RoleManager<IdentityRole> roleManager, IMapper mapper)
    {
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var model = await _roleManager.Roles.OrderBy(x => x.Name).ToListAsync();

        var map = _mapper.Map<List<IdentityRole>, List<RoleVM>>(model);

        return View(map);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create(RoleVM model)
    {
        var role = new IdentityRole
        {
            Name = model.Name
        };

        var result = await _roleManager.CreateAsync(role);

        if (result.Succeeded)
        {
            return RedirectToAction("Index");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("Name", error.Description);
        }
        return View();
    }

    public async Task<IActionResult> Update(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        if (role == null)
        {
            return NotFound();
        }

        var map = _mapper.Map<IdentityRole, RoleVM>(role);

        return View(map);
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Update(RoleVM model)
    {
        var role = await _roleManager.FindByIdAsync(model.Id);

        if (role == null)
        {
            return NotFound();
        }

        role.Name = model.Name;

        var result = await _roleManager.UpdateAsync(role);

        if (result.Succeeded)
        {
            return RedirectToAction("Index");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("Name", error.Description);
        }

        return View(model);
    }

    [HttpDelete]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        if (role == null)
        {
            return Ok(new ResponseDTO { IsSuccess = false, Message = "Role not deleted" });
        }

        var result = await _roleManager.DeleteAsync(role);

        if (result.Succeeded)
            return Ok(new ResponseDTO { IsSuccess = result.Succeeded, Message = "Role deleted successfully" });
        else
            return Ok(new ResponseDTO { IsSuccess = result.Succeeded, Message = "Role not deleted" });
    }
}