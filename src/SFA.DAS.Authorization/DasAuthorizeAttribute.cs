using System;

namespace SFA.DAS.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    public class DasAuthorizeAttribute : Attribute
    {
        public string[] Options { get; }

        public DasAuthorizeAttribute(params string[] options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Options = options;
        }
    }
}