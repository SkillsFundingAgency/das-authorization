#if NETCOREAPP2_0
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using SFA.DAS.Authorization.Mvc.Attributes;

namespace SFA.DAS.Authorization.Mvc.Extensions
{
    public static class StaffActionDescriptorExtensions
    {
        private static readonly ConcurrentDictionary<string, List<StaffAuthorizeAttribute>> Cache = new ConcurrentDictionary<string, List<StaffAuthorizeAttribute>>();

        public static IReadOnlyCollection<StaffAuthorizeAttribute> GetStaffAuthorizeAttributes(this ControllerActionDescriptor actionDescriptor)
        {
            var key = $"{actionDescriptor.ControllerName}.{actionDescriptor.ActionName}";

            return Cache.GetOrAdd(key, k =>
            {
                var actionAttributes = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(StaffAuthorizeAttribute), true).Cast<StaffAuthorizeAttribute>();
                var controllerAttributes = actionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(StaffAuthorizeAttribute), true).Cast<StaffAuthorizeAttribute>();
                var attributes = actionAttributes.Concat(controllerAttributes).ToList();

                return attributes;
            });
        }
    }
}
#elif NET462
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SFA.DAS.Authorization.Mvc.Attributes;

namespace SFA.DAS.Authorization.Mvc.Extensions
{
    public static class StaffActionDescriptorExtensions
    {
        private static readonly ConcurrentDictionary<string, List<StaffAuthorizeAttribute>> Cache = new ConcurrentDictionary<string, List<StaffAuthorizeAttribute>>();

        public static IReadOnlyCollection<StaffAuthorizeAttribute> GetStaffAuthorizeAttributes(this ActionDescriptor actionDescriptor)
        {
            var key = $"{actionDescriptor.ControllerDescriptor.ControllerName}.{actionDescriptor.ActionName}";

            return Cache.GetOrAdd(key, k =>
            {
                var actionAttributes = actionDescriptor.GetCustomAttributes(typeof(StaffAuthorizeAttribute), true).Cast<StaffAuthorizeAttribute>();
                var controllerAttributes = actionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(DasAuthorizeAttribute), true).Cast<StaffAuthorizeAttribute>();
                var attributes = actionAttributes.Concat(controllerAttributes).ToList();
                
                return attributes;
            });
        }
    }
}
#endif