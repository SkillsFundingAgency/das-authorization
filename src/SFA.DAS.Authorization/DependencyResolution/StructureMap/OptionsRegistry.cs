using Microsoft.Extensions.Options;
using StructureMap;

namespace SFA.DAS.Authorization.DependencyResolution.StructureMap
{
    internal class OptionsRegistry : Registry
    {
        public OptionsRegistry()
        {
            For(typeof(IOptions<>)).Use(typeof(OptionsManager<>)).Singleton();
            For(typeof(IOptionsFactory<>)).Use(typeof(OptionsFactory<>)).Transient();
            For(typeof(IOptionsMonitor<>)).Use(typeof(OptionsMonitor<>)).Singleton();
            For(typeof(IOptionsMonitorCache<>)).Use(typeof(OptionsCache<>)).Singleton();
            For(typeof(IOptionsSnapshot<>)).Use(typeof(OptionsManager<>));
        }
    }
}