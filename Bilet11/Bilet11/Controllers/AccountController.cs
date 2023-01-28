using Bilet11.Models;
using Bilet11.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Bilet11.Utilities.Enums;
namespace Bilet11.Controllers
{
    public class AccountController : Controller
    {
        UserManager<AppUser> _userManager { get; }
        SignInManager<AppUser> _signinManager { get; }
        RoleManager<IdentityRole> _roleManager { get; }

        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signinManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signinManager = signinManager;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Register));
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByNameAsync(registerVM.Username);
            if (user != null)
            {
                ModelState.AddModelError("Username", "This username is already taken!");
                return View();
            }
            user = new AppUser()
            {
                FirstName = registerVM.Name,
                LastName = registerVM.Surname,
                UserName = registerVM.Username,
                Email = registerVM.Email,
            };
            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            //Admin panel ucun test etmek ucun avtomatik register olunan hesablara admin rolunun verilmesi
            await _userManager.AddToRoleAsync(user,Roles.Admin.ToString());
            return RedirectToAction(nameof(Login));
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVM loginVM,string? returnUrl)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
            if (user is null)
            {
                user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
                if (user is null)
                {
                    ModelState.AddModelError("", "Login or password is incorrect!");
                    return View();
                }
            }
                var result = await _signinManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsPersistance, true);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Login or password is incorrect!");
                    return View();
                }
                if (returnUrl is null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(returnUrl);
                }
        }
        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        // Rollarin yaradilib elave edilmesi ucun yaradilmisdir.
        public async Task<IActionResult> AddRoles()
        {
            foreach (var item in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
