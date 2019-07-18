using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using StructureMap;

namespace SFA.DAS.Authorization.DependencyResolution
{
    public class LoggerRegistry : Registry
    {
        public LoggerRegistry()
        {
            For<ILoggerFactory>().Use(() => new LoggerFactory().AddNLog()).Singleton();
            For(typeof(ILogger<>)).Use(typeof(Logger<>)).Singleton();
        }
    }
}