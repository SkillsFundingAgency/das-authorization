#if NETCOREAPP2_0
using System;
using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.Authorization.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class DasAuthorizeAttribute : AuthorizeAttribute
    {
        public string[] Options { get; }

        public DasAuthorizeAttribute(params string[] options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }
    }
}
#elif NET462
using System;
using System.Web.Mvc;

namespace SFA.DAS.Authorization.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class DasAuthorizeAttribute : AuthorizeAttribute
    {
        public string[] Options { get; }

        public DasAuthorizeAttribute(params string[] options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }
    }
}
#endif