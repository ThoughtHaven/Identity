using System;
using Microsoft.Extensions.Logging;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakeLoggerFactory : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider) => throw new System.NotImplementedException();
        public ILogger CreateLogger(string categoryName) => new Logger();
        public void Dispose() => throw new System.NotImplementedException();

        private class Logger : ILogger
        {
            public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException();
            public bool IsEnabled(LogLevel logLevel) => false;
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
                Exception exception, Func<TState, Exception, string> formatter) =>
                new NotImplementedException();
        }
    }
}