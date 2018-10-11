#if NET462
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SFA.DAS.Authorization.Mvc
{
    public static class ActionDescriptorExtensions
    {
        private static readonly ConcurrentDictionary<string, List<DasAuthorizeAttribute>> Cache = new ConcurrentDictionary<string, List<DasAuthorizeAttribute>>();

        public static IEnumerable<DasAuthorizeAttribute> GetDasAuthorizeAttributes(this ActionDescriptor actionDescriptor)
        {
            var key = $"{actionDescriptor.ControllerDescriptor.ControllerName}.{actionDescriptor.ActionName}";

            return Cache.GetOrAdd(key, k =>
            {
                var attributes = new List<DasAuthorizeAttribute>();
                var actionAttribute = actionDescriptor.GetCustomAttributes(typeof(DasAuthorizeAttribute), false).Cast<DasAuthorizeAttribute>().SingleOrDefault();
                var controllerAttribute = actionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(DasAuthorizeAttribute), false).Cast<DasAuthorizeAttribute>().SingleOrDefault();

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