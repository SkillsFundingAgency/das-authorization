using System;
using System.Web.Mvc;

namespace SFA.DAS.Authorization.Mvc
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