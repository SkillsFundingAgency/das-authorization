using System;
using System.Collections.Generic;

namespace SFA.DAS.Authorization
{
    internal static class EnumerableExtensions
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

        public static string ToCsvString<T>(this IEnumerable<T> enumerable)
        {
            var first = true;
            var result = String.Empty;
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