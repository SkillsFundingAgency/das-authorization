using System.Web.Mvc;
using SFA.DAS.Authorization.Mvc.Extensions;

namespace SFA.DAS.Authorization.NetFrameworkTestHarness
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.AddAuthorizationFilter();
            filters.AddUnauthorizedAccessExceptionFilter();
        }
    }
}
