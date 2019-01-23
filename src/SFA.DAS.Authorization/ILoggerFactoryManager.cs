using System;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Authorization
{
    public interface ILoggerFactoryManager
    {
        ILoggerFactory GetFactory(Func<ILoggerFactory> iocProvidedFactoryFunction);
    }
}