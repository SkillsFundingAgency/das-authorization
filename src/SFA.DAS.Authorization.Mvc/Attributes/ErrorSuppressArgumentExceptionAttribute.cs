using System;

namespace SFA.DAS.Authorization.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ErrorSuppressArgumentExceptionAttribute : Attribute
    {
    }
}
