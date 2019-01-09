using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace SFA.DAS.Authorization
{
    public interface ILoggerFactoryManager
    {
        ILoggerFactory GetFactory(Func<ILoggerFactory> iocProvidedFactoryFunction);
    }

    public class LoggerFactoryManager : ILoggerFactoryManager
    {
        private ILoggerFactory _factory;

        public ILoggerFactory GetFactory(Func<ILoggerFactory> iocProvidedFactoryFunction)
        {
            if (_factory == null)
            {
                try
                {
                    _factory = iocProvidedFactoryFunction.Invoke();
                }
                catch (Exception)
                {
                    _factory = null;
                }
            }

            if (_factory == null)
            {
                _factory = new LoggerFactory().AddNLog();
            }

            return _factory;
        }
    }
}
