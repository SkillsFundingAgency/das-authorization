using System.Web.Mvc;
using System.Web.Security;

namespace SFA.DAS.Authorization.NetFrameworkTestHarness.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            FormsAuthentication.SetAuthCookie(Models.User.Email, false);
            
            return RedirectToAction("Index", "Home");
        }
        
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Http401()
        {
            return View();
        }

        public ActionResult Http403()
        {
            return View();
        }
    }
}