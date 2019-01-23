using System.Web.Mvc;
using System.Web.Security;

namespace SFA.DAS.Authorization.TestHarness.Controllers
{
    public class AccountController : Controller
    {public ActionResult Login()
        {
            FormsAuthentication.SetAuthCookie(Usernames.Default, false);
            
            return RedirectToAction("Index", "Home");
        }
        
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Unauthenticated()
        {
            return View();
        }

        public ActionResult Unauthorized()
        {
            return View();
        }
    }
}