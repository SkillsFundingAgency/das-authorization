using Microsoft.Extensions.Caching.Memory;
using StructureMap;

namespace SFA.DAS.Authorization
{
    public class MemoryCacheRegistry : Registry
    {
        public MemoryCacheRegistry()
        {
            For<IMemoryCache>().Use<MemoryCache>().Singleton();
        }
    }
}