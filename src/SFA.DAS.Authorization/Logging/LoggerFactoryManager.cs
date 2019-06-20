using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace SFA.DAS.Authorization.Logging
{
    public class LoggerFactoryManager : ILoggerFactoryManager
    {
        private readonly ILoggerFactory _loggerFactory;

        public LoggerFactoryManager(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? new LoggerFactory().AddNLog();
        }

        public ILoggerFactory GetLoggerFactory()
        {
            return _loggerFactory;
        }
    }
}