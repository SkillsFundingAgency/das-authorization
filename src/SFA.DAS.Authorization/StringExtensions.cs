using System;

namespace SFA.DAS.Authorization
{
    internal static class StringExtensions
    {
        public static T ToEnum<T>(this string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}