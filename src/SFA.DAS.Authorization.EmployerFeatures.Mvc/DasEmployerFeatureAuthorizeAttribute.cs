using System;
using SFA.DAS.Authorization.Mvc;

namespace SFA.DAS.Authorization.EmployerFeatures.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class DasEmployerFeatureAuthorizeAttribute
    : DasAuthorizeAttribute
    {
        public DasEmployerFeatureAuthorizeAttribute(Feature toggledFeature)
        :base(new string[]{EmployerFeature.Prefix + toggledFeature})
        {
        }
    }
}