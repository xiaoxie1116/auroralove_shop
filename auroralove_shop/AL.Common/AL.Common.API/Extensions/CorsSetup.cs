using AL.Common.Tools;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.Common.API.Extensions
{
    /// <summary>
    /// cors 跨域服务启动
    /// </summary>
    public static class CorsSetup
    {
        public static void AddCorsSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            string[] ips = LocalAppsetting.GetSettingNode(new string[] { "Startup", "Cors", "IPs" }).Split(',');
            if (ips.Length > 0)
                services.AddCors(c =>
                {
                    c.AddPolicy("CustomCorsPolicy", policy =>
                    {

                        // 支持多个域名端口，注意端口号后不要带/斜杆：比如localhost:8000/，是错的,同时区别http和https
                        // 注意，http://127.0.0.1:44308 和 http://localhost:44308 是不一样的，尽量写两个
                        policy
                            //设定允许跨域的来源，有多个可以用','隔开
                            .WithOrigins(ips)
                            .AllowAnyHeader()//Ensures that the policy allows any header.
                            .AllowAnyMethod();
                    });

                    // 允许任意跨域请求，也要配置中间件(生产环境慎用)
                    //c.AddPolicy("AllRequests",policy=> {
                    //    policy.AllowAnyOrigin();
                    //    policy.AllowAnyMethod();
                    //    policy.AllowAnyHeader();
                    //});
                });

        }

    }
}
