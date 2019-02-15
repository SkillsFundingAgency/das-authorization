using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Authorization.NetCoreTestHarness.Controllers
{
    public class AccountController : Controller
    {
        public async Task<IActionResult> Login()
        {
            var claims = new[] { new Claim(ClaimTypes.Name, Models.User.Email) };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            
            return RedirectToAction("Index", "Home");
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Http401()
        {
            return View();
        }

        public IActionResult Http403()
        {
            return View();
        }
    }
}