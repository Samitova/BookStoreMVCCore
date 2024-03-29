
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SmartBreadcrumbs.Extensions;
using NLog.Extensions.Logging;

namespace BookStore.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();   
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, loggin) =>
                {
                    loggin.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    loggin.AddConsole();
                    loggin.AddDebug();
                    loggin.AddEventSourceLogger();
                    loggin.AddNLog();
                }).ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
