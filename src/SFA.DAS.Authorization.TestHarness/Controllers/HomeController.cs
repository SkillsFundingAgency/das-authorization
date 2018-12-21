using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.Authorization.TestHarness.Models;

namespace SFA.DAS.Authorization.TestHarness.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public HomeController()
        {
        }

        public HomeController(ApplicationSignInManager signInManager, ApplicationUserManager userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public ApplicationSignInManager SignInManager {
            get {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager {
            get {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        [DasAuthorize(TestOptions.DasAuthorizeTrue)]
        public ActionResult DasAuthorizeTrue()
        {
            ViewBag.Message = "DasAuthorizeTrue";

            return View();
        }

        [DasAuthorize(TestOptions.DasAuthorizeFalse)]
        public ActionResult DasAuthorizeFalse()
        {
            ViewBag.Message = "DasAuthorizeFalse";

            return View();
        }

        [DasAuthorize(TestOptions.DasAuthorizeFalseWithErrors)]
        public ActionResult DasAuthorizeFalseWithErrors()
        {
            ViewBag.Message = "DasAuthorizeFalseWithErrors";

            return View();
        }

        [DasAuthorize(EmployerFeatures.EmployerFeature.ProviderRelationships)]
        public ActionResult DasAuthorizeEmployerFeaturesProviderRelationships()
        {
            ViewBag.Message = "DasAuthorizeEmployerFeaturesProviderRelationships";

            return View("GenericAuthorizationLandingPage");
        }

        public async Task<ActionResult> AutoLogin()
        {
            const string username = "test@example.com";
            var user = UserManager.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                user = new ApplicationUser { UserName = username, Email = username };
                var result = await UserManager.CreateAsync(user, "Test11!!");
                if (!result.Succeeded)
                {
                    throw new Exception("Error automatically registering a user in test harness");
                }
            }

            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            return RedirectToAction("Index", "Home");
        }

    }
}