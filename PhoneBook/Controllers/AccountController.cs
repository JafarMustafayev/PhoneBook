namespace PhoneBook.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMailService _emailSender;

    public AccountController(UserManager<AppUser> userManager,
                          RoleManager<IdentityRole> roleManager,
                          SignInManager<AppUser> signInManager,
                          IMailService emailSender)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
    }

    public IActionResult Login(string? ReturnUrl)
    {
        if(ReturnUrl != null) return View(new LoginVM { ReturnUrl = ReturnUrl });
        

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.EmailOrUsername);
        if (user == null)
        {
            user = await _userManager.FindByNameAsync(model.EmailOrUsername);
            if (user == null)
            {
                ModelState.AddModelError("", "Username or Password is invalid");
                return View(model);
            }
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Username or Password is invalid");

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError("", "User not found");
            return View(model);
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var callbackUrl = Url.Action("ResetPassword", "Auth", new { token, email = user.Email }, Request.Scheme);

        var message = new MailRequest()
        {
            Attachments = null,
            Body = $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>",
            Subject = "Reset Password",
            ToEmail = user.Email
        };

        await _emailSender.SendEmailAsync(message);

        return RedirectToAction("Login");
    }
}