using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace EmptyAspNetCore
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddProvider(new FileLoggerProvider("D://123.txt"));
            app.Run(async (context) =>
            {
                // создаем объект логгера
                var logger = loggerFactory.CreateLogger("RequestInfoLogger");
                // пишем на консоль информацию
                logger.LogInformation("Processing request {0}", context.Request.Path);

                await context.Response.WriteAsync("Hello World!");
            });
        }


        public class FileLogger : ILogger
        {
            private string filePath;
            private object _lock = new object();
            public FileLogger(string path)
            {
                filePath = path;
            } 
            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                //return logLevel == LogLevel.Trace;
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (formatter != null)
                {
                    lock (_lock)
                    {
                        File.AppendAllText(filePath, formatter(state, exception) + Environment.NewLine);
                    }
                }
            }
        }

        public class FileLoggerProvider : ILoggerProvider
        {
            private string path;
            public FileLoggerProvider(string _path)
            {
                path = _path;
            }
            public ILogger CreateLogger(string categoryName)
            {
                return new FileLogger(path);
            }

            public void Dispose()
            {
            }
        }
    }
}