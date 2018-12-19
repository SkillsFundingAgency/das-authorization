using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SFA.DAS.Authorization.Mvc;

namespace SFA.DAS.Authorization.TestHarness.Controllers
{
    [DasAuthorize(TestOptions.DasAuthorizeTrue)]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [DasAuthorize(TestOptions.DasAuthorizeTrue)]
        public ActionResult DasAuthorizeTrue()
        {
            ViewBag.Message = "This action is set up to pass DAS authorization.";

            return View();
        }

        [DasAuthorize(TestOptions.DasAuthorizeFalse)]
        public ActionResult DasAuthorizeFalse()
        {
            ViewBag.Message = "This action is set up to fail DAS authorization.";

            return View();
        }

        [DasAuthorize(TestOptions.DasAuthorizeFalseWithErrors)]
        public ActionResult DasAuthorizeFalseWithErrors()
        {
            ViewBag.Message = "This action is set up to fail DAS authorization with errors.";

            return View();
        }
    }
}