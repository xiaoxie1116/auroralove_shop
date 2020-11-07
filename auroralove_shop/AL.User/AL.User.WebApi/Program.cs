using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AL.User.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //���autofac �ķ��񹤳�
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                //log4net ����
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    loggingBuilder.AddFilter("System", LogLevel.Information);
                    loggingBuilder.AddFilter("Microsoft", LogLevel.Information);
                    var path = context.HostingEnvironment.ContentRootPath;
                    loggingBuilder.AddLog4Net($"{path}/log4net.config");//�����ļ�
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://localhost:6001");
                });
    }
}
