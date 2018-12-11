using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.Authorization
{
    public static class EnumerableExtensions
    {
        public static string ToCsvString<T>(this IEnumerable<T> enumerable)
        {
            var first = true;
            var result = string.Empty;
            foreach (var item in enumerable)
            {
                if (!first)
                    result += ", ";
                first = false;
                result += item.ToString();
            }

            return result;
        }
    }
}
