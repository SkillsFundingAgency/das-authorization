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
            return Cache.GetOrAdd(actionDescriptor.UniqueId, k =>
            {
                var attributeType = typeof(DasAuthorizeAttribute);
                var attributes = new List<DasAuthorizeAttribute>();
                var actionAttribute = actionDescriptor.GetCustomAttributes(attributeType, true).Cast<DasAuthorizeAttribute>().SingleOrDefault();
                var controllerAttribute = actionDescriptor.ControllerDescriptor.GetCustomAttributes(attributeType, true).Cast<DasAuthorizeAttribute>().SingleOrDefault();

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