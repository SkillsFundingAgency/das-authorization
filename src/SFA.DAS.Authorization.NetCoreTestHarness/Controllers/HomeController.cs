using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.CommitmentPermissions.Options;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.Authorization.NetCoreTestHarness.Authorization;
using SFA.DAS.Authorization.ProviderPermissions.Options;

namespace SFA.DAS.Authorization.NetCoreTestHarness.Controllers
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

        [DasAuthorize(CommitmentOperation.AccessCohort)]
        public ActionResult CommitmentOperationAccessCohort()
        {
            return View("Authorized");
        }

        [DasAuthorize("EmployerFeature.ProviderRelationships")]
        public ActionResult EmployerFeatureProviderRelationships()
        {
            return View("Authorized");
        }

        [DasAuthorize(EmployerUserRole.Owner)]
        public ActionResult EmployerUserRoleOwner()
        {
            return View("Authorized");
        }

        [DasAuthorize(EmployerUserRole.Any)]
        public ActionResult EmployerUserRoleAny()
        {
            return View("Authorized");
        }

        [DasAuthorize("Feature.ProviderRelationships")]
        public ActionResult FeatureProviderRelationships()
        {
            return View("Authorized");
        }

        [DasAuthorize("ProviderFeature.ProviderRelationships")]
        public ActionResult ProviderFeatureProviderRelationships()
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