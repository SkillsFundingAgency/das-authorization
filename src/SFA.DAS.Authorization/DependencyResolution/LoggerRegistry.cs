using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.Logging;
using StructureMap;

namespace SFA.DAS.Authorization.DependencyResolution
{
    public class LoggerRegistry : Registry
    {
        public LoggerRegistry()
        {
            For<ILoggerFactoryManager>().Use(c => new LoggerFactoryManager(c.TryGetInstance<ILoggerFactory>())).Singleton();
            For(typeof(ILogger<>)).Use(typeof(Logger<>)).Ctor<ILoggerFactory>().Is(c => c.GetInstance<ILoggerFactoryManager>().GetLoggerFactory());
        }
    }
}