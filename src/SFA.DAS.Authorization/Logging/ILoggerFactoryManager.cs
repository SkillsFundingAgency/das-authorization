using Microsoft.Extensions.Logging;

namespace SFA.DAS.Authorization.Logging
{
    public interface ILoggerFactoryManager
    {
        ILoggerFactory GetLoggerFactory();
    }
}