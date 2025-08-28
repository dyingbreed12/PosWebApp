using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PosWebApp.Models.ViewModel;
using PosWebAppBusinessLogic.Interfaces;
using PosWebAppCommon.Models;
using System.Security.Claims;

namespace PosWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepo;

        public AccountController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _userRepo.GetUserByUsername(vm.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(vm.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(vm);
            }

            // Pass the full user object to SignInUser
            await SignInUser(user);

            // Redirect based on role
            if (user.Role == "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Sales"); // Redirect Counter to the Sales page
            }
        }


        private async Task SignInUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) // Add the user's ID
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult Denied() => Content("Access Denied");
    }
}
