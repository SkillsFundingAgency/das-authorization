#if NETCOREAPP2_0
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using SFA.DAS.Authorization.Mvc.Attributes;

namespace SFA.DAS.Authorization.Mvc.Extensions
{
    public static class ActionDescriptorExtensions
    {
        private static readonly ConcurrentDictionary<string, List<DasAuthorizeAttribute>> Cache = new ConcurrentDictionary<string, List<DasAuthorizeAttribute>>();

        public static IReadOnlyCollection<DasAuthorizeAttribute> GetDasAuthorizeAttributes(this ControllerActionDescriptor actionDescriptor)
        {
            var key = $"{actionDescriptor.ControllerName}.{actionDescriptor.ActionName}";

            return Cache.GetOrAdd(key, k =>
            {
                var actionAttributes = actionDescriptor.MethodInfo.GetCustomAttributes(typeof(DasAuthorizeAttribute), true).Cast<DasAuthorizeAttribute>();
                var controllerAttributes = actionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(DasAuthorizeAttribute), true).Cast<DasAuthorizeAttribute>();
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
    public static class ActionDescriptorExtensions
    {
        private static readonly ConcurrentDictionary<string, List<DasAuthorizeAttribute>> Cache = new ConcurrentDictionary<string, List<DasAuthorizeAttribute>>();

        public static IReadOnlyCollection<DasAuthorizeAttribute> GetDasAuthorizeAttributes(this ActionDescriptor actionDescriptor)
        {
            var key = $"{actionDescriptor.ControllerDescriptor.ControllerName}.{actionDescriptor.ActionName}";

            return Cache.GetOrAdd(key, k =>
            {
                var actionAttributes = actionDescriptor.GetCustomAttributes(typeof(DasAuthorizeAttribute), true).Cast<DasAuthorizeAttribute>();
                var controllerAttributes = actionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(DasAuthorizeAttribute), true).Cast<DasAuthorizeAttribute>();
                var attributes = actionAttributes.Concat(controllerAttributes).ToList();
                
                return attributes;
            });
        }
    }
}
#endif