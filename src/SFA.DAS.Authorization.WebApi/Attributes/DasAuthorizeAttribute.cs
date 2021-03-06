using System;
using System.Web.Http;

namespace SFA.DAS.Authorization.WebApi.Attributes
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