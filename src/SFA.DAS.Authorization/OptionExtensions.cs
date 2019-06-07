using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Authorization
{
    internal static class OptionExtensions
    {
        public static void EnsureNoAndOptions(this IReadOnlyCollection<string> options)
        {
            if (options.Count > 1)
            {
                throw new NotImplementedException("Combining options (to specify AND) is not currently supported");
            }
        }

        public static void EnsureNoOrOptions(this IReadOnlyCollection<string> options)
        {
            if (options.Any(o => o.Contains(',')))
            {
                throw new NotImplementedException("Combining options (to specify OR) by comma separating them is not currently supported");
            }
        }

        public static bool IsSameAs<T>(this IReadOnlyCollection<T> lhs, IReadOnlyCollection<T> rhs)
        {
            if (ReferenceEquals(lhs, rhs))
            {
                return true;
            }

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null) || lhs.Count != rhs.Count)
            {
                return false;
            }

            return lhs.SequenceEqual(rhs);
        }
    }
}