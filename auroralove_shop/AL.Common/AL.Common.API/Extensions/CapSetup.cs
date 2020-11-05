using AL.Common.Tools;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AL.Common.API.Extensions
{
    /// <summary>
    /// CAP 分布式事务
    /// </summary>
    public static class CapSetup
    {
        public static void AddCapSetup(this IServiceCollection services)
        {
            var _connectionWriteStr = LocalAppsetting.GetSettingNode(new string[] { "AppSettings", "MySql", "Write", "ConnectionString" });
            var dbName = LocalAppsetting.GetSettingNode(new string[] { "Startup", "RabbitMqDb" });
            var rabbit = LocalAppsetting.GetSettingNode<CapConfigModel>("Startup", "RabbitMq")?.FirstOrDefault();
            if (rabbit == null) throw new Exception("rabbitmq消息队列的配置为空，请配置！");
            services.AddCap(x =>
            {
                // 注册 Dashboard 面板
                x.UseDashboard();
                x.UseMySql(string.Format(_connectionWriteStr, dbName));
                //CAP支持 RabbitMQ、Kafka、AzureServiceBus 等作为MQ，根据使用选择配置：
                x.UseRabbitMQ(options =>
                {
                    options.UserName = rabbit.UserName;//用户名
                    options.Password = rabbit.Password;//密码
                    options.HostName = rabbit.HostName;//rabbitmq ip
                    if (!string.IsNullOrEmpty(rabbit.Port))
                        options.Port = rabbit.Port.ToInt();
                });
            });

        }


    }

    public class CapConfigModel
    {
        public string HostName { get; set; }

        public string Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }


    }
}
