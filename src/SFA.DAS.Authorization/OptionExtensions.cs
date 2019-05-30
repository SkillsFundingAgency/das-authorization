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
    }
}