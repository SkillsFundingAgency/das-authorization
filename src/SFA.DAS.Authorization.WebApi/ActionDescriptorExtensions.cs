#if NET462
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;

namespace SFA.DAS.Authorization.WebApi
{
    public static class ActionDescriptorExtensions
    {
        private static readonly ConcurrentDictionary<string, List<DasAuthorizeAttribute>> Cache = new ConcurrentDictionary<string, List<DasAuthorizeAttribute>>();

        public static IEnumerable<DasAuthorizeAttribute> GetDasAuthorizeAttributes(this HttpActionDescriptor actionDescriptor)
        {
            var key = $"{actionDescriptor.ControllerDescriptor.ControllerName}.{actionDescriptor.ActionName}";

            return Cache.GetOrAdd(key, k =>
            {
                var attributes = new List<DasAuthorizeAttribute>();
                var actionAttribute = actionDescriptor.GetCustomAttributes<DasAuthorizeAttribute>(true).SingleOrDefault();
                var controllerAttribute = actionDescriptor.ControllerDescriptor.GetCustomAttributes<DasAuthorizeAttribute>(true).SingleOrDefault();

                if (actionAttribute != null)
                {
                    attributes.Add(actionAttribute);
                }

                if (controllerAttribute != null)
                {
                    attributes.Add(controllerAttribute);
                }

                return attributes;
            });
        }
    }
}
#endif