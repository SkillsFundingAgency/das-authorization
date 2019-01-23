using System.Web.Mvc;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.Authorization.NetFrameworkTestHarness.Authorization;
using SFA.DAS.Authorization.ProviderPermissions;

namespace SFA.DAS.Authorization.NetFrameworkTestHarness.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        [DasAuthorize(TestOption.Authorized)]
        public ActionResult TestOptionAuthorized()
        {
            return View("Authorized");
        }

        [DasAuthorize(TestOption.UnauthorizedSingleError)]
        public ActionResult TestOptionUnauthorizedSingleError()
        {
            return View("Authorized");
        }

        [DasAuthorize(TestOption.UnauthorizedMultipleErrors)]
        public ActionResult TestOptionUnauthorizedMultipleErrors()
        {
            return View("Authorized");
        }

        [DasAuthorize(EmployerFeature.ProviderRelationships)]
        public ActionResult EmployerFeatureProviderRelationships()
        {
            return View("Authorized");
        }

        [DasAuthorize(EmployerUserRole.Owner)]
        public ActionResult EmployerUserRoleOwner()
        {
            return View("Authorized");
        }

        [DasAuthorize(ProviderOperation.CreateCohort)]
        public ActionResult ProviderOperationCreateCohort()
        {
            return View("Authorized");
        }
    }
}