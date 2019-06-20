using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using SFA.DAS.Authorization.WebApi.Attributes;

namespace SFA.DAS.Authorization.WebApi.Extensions
{
    public static class ActionDescriptorExtensions
    {
        private static readonly ConcurrentDictionary<string, List<DasAuthorizeAttribute>> Cache = new ConcurrentDictionary<string, List<DasAuthorizeAttribute>>();

        public static IReadOnlyCollection<DasAuthorizeAttribute> GetDasAuthorizeAttributes(this HttpActionDescriptor actionDescriptor)
        {
            var key = $"{actionDescriptor.ControllerDescriptor.ControllerName}.{actionDescriptor.ActionName}";

            return Cache.GetOrAdd(key, k =>
            {
                var actionAttributes = actionDescriptor.GetCustomAttributes<DasAuthorizeAttribute>(true);
                var controllerAttributes = actionDescriptor.ControllerDescriptor.GetCustomAttributes<DasAuthorizeAttribute>(true);
                var attributes = actionAttributes.Concat(controllerAttributes).ToList();

                return attributes;
            });
        }
    }
}