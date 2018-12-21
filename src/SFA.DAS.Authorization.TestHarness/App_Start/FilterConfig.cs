using System.Web.Mvc;
using SFA.DAS.Authorization.Mvc;

namespace SFA.DAS.Authorization.TestHarness
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.AddAuthorizationFilter();
        }
    }
}
