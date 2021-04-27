using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace DynamicRouting
{
    public class Program
    {
     
            public async static Task Main(string[] args)
            {
                var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
                try
                {
                    string environmentName = "Debug";
#if DEBUG
                    environmentName = "Debug";
#endif
#if TEST
                environmentName = "Test";
#endif
#if RELEASE
                environmentName = "Release";
#endif
                    var host = CreateHostBuilder(args).UseEnvironment(environmentName).Build();
                    await host.InitAsync();
                    host.Run();
                }
                catch (Exception exception)
                {
                    //NLog: catch setup errors
                    logger.Error(exception, "Stopped program because of exception");
                    throw;
                }
                finally
                {
                    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                    NLog.LogManager.Shutdown();
                }
            }

            public static IWebHostBuilder CreateHostBuilder(string[] args) =>
                WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
               .UseStartup<DynamicRoutingStartup>()
                .UseNLog();
        }
    
}
