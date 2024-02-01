using Microsoft.AspNetCore.Authorization;

namespace PhoneBook.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class UserManageController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMapper _mapper;

    public UserManageController(UserManager<AppUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                SignInManager<AppUser> signInManager,
                                IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    #region Index

    public async Task<IActionResult> Index()
    {
        List<SingleUserVM> map = new();
        var users = await _userManager.Users.ToListAsync();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Count > 0)
            {
                map.Add(new SingleUserVM
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = roles[0]
                });
            }
        }

        return View(map);
    }

    #endregion Index

    #region Create

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create(UserCreateVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new AppUser
        {
            UserName = model.Username,
            Email = model.Email,
            FullName = model.FullName,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            result = await _userManager.AddToRoleAsync(user, "Admin");
            return RedirectToAction("Index");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(model);
    }

    #endregion Create

    #region Delete

    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);

        var role = await _userManager.GetRolesAsync(user);

        if (role != null && role.Contains("SuperAdmin"))
        {
            var userToDelete = await _userManager.FindByIdAsync(id);
            if (userToDelete == null)
            {
                return Ok(new ResponseDTO { IsSuccess = false, Message = "İstifadəçi tapılmadı" });
            }

            var res = await _userManager.DeleteAsync(userToDelete);
            if (res.Succeeded)
            { return Ok(new ResponseDTO { IsSuccess = true, Message = "İstifadəçi uğurla silindi" }); }
            else
            { return Ok(new ResponseDTO { IsSuccess = false, Message = "İstifadəçi silinə bilmədi" }); }
        }
        else
        {
            return Ok(new ResponseDTO { IsSuccess = false, Message = "Istifadəçini silmək icazəniz yoxdur" });
        }
    }

    #endregion Delete

    #region IsSuperAdmin

    [HttpGet]
    public async Task<IActionResult> IsSuperAdmin()
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);

        var role = await _userManager.GetRolesAsync(user);

        if (role != null && role.Contains("SuperAdmin"))
        {
            return Ok(true);
        }
        else
        {
            return Ok(false);
        }
    }

    #endregion IsSuperAdmin
}