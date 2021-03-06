using Microsoft.Extensions.Caching.Memory;
using StructureMap;

namespace SFA.DAS.Authorization.DependencyResolution.StructureMap
{
    internal class MemoryCacheRegistry : Registry
    {
        public MemoryCacheRegistry()
        {
            For<IMemoryCache>().Use<MemoryCache>().Singleton();
        }
    }
}