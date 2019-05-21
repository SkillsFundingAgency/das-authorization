using System.Web.Http;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.Authorization.NetFrameworkTestHarness.Authorization;
using SFA.DAS.Authorization.ProviderPermissions;
using SFA.DAS.Authorization.WebApi;

namespace SFA.DAS.Authorization.NetFrameworkTestHarness.Api.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Index()
        {
            return Ok();
        }
        
        [HttpGet]
        [DasAuthorize(TestOption.Authorized)]
        public IHttpActionResult TestOptionAuthorized()
        {
            return Ok("Authorized");
        }

        [HttpGet]
        [DasAuthorize(TestOption.UnauthorizedSingleError)]
        public IHttpActionResult TestOptionUnauthorizedSingleError()
        {
            return Ok("Authorized");
        }

        [HttpGet]
        [DasAuthorize(TestOption.UnauthorizedMultipleErrors)]
        public IHttpActionResult TestOptionUnauthorizedMultipleErrors()
        {
            return Ok("Authorized");
        }

        [HttpGet]
        [DasAuthorize("EmployerFeature.ProviderRelationships")]
        public IHttpActionResult EmployerFeatureProviderRelationships()
        {
            return Ok("Authorized");
        }

        [HttpGet]
        [DasAuthorize(EmployerUserRole.Owner)]
        public IHttpActionResult EmployerUserRoleOwner()
        {
            return Ok("Authorized");
        }

        [HttpGet]
        [DasAuthorize(EmployerUserRole.Any)]
        public IHttpActionResult EmployerUserRoleAny()
        {
            return Ok("Authorized");
        }

        [HttpGet]
        [DasAuthorize("Feature.ProviderRelationships")]
        public IHttpActionResult FeatureProviderRelationships()
        {
            return Ok("Authorized");
        }

        [HttpGet]
        [DasAuthorize("ProviderFeature.ProviderRelationships")]
        public IHttpActionResult ProviderFeatureProviderRelationships()
        {
            return Ok("Authorized");
        }

        [HttpGet]
        [DasAuthorize(ProviderOperation.CreateCohort)]
        public IHttpActionResult ProviderOperationCreateCohort()
        {
            return Ok("Authorized");
        }
    }
}