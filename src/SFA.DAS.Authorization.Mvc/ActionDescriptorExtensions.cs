using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SFA.DAS.Authorization.Mvc
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