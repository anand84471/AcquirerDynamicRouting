using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
namespace TorrentPayController
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    CreateHostBuilder(args).Build().Run();
        //    //var host = new WebHostBuilder().UseKestrel().UseStartup<TorrentPayStartup>().UseUrls("http://*:4000").Build();
        //    //host.Run();
        //}
        public  static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
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
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging((hostingContext, logging) =>
            {
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                logging.AddDebug();
                logging.AddNLog();
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TorrentPayStartup>();
                    //webBuilder.UseUrls("http://*:2000");
                });
            
        //public static IHostBuilder CreateHostBuilder();
        //   public static IHostBuilder CreateHostBuilder(string[] args) =>
        //Host.CreateDefaultBuilder(args)
        //    .ConfigureServices((context, services) =>
        //    {
        //        services.Configure<KestrelServerOptions>(
        //            context.Configuration.GetSection("Kestrel"));
        //    })
        //    .ConfigureWebHostDefaults(webBuilder =>
        //    {
        //        webBuilder.UseStartup<TorrentPayStartup>();
        //    });
    }
}
