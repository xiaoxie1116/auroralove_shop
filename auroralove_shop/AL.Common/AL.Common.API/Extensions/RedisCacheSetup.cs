using AL.Common.Tools;
using AL.Common.Tools.Redis;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.Common.API.Extensions
{
    /// <summary>
    /// Redis 缓存
    /// </summary>
    public static class RedisCacheSetup
    {

        public static void AddRedisCacheSetup(this IServiceCollection services)
        {
            string redisConfiguration = Appsettings.GetSettingNode(new string[] { "AppSettings", "Redis", "ConnectionString" });//获取连接字符串
            //实例名称，在key前面会加前缀                                                                                                                                                                                                                                 
            string alias = Appsettings.GetSettingNode(new string[] { "AppSettings", "Redis", "PrefixAlias" });
            int defaultDb = Appsettings.GetSettingNode(new string[] { "AppSettings", "Redis", "DefaultDb" }).ObjToInt();
            string passWord = Appsettings.GetSettingNode(new string[] { "AppSettings", "Redis", "PassWord" });
            if (string.IsNullOrWhiteSpace(redisConfiguration))
            {
                throw new ArgumentException("redis 连接字符串为空！", nameof(redisConfiguration));
            }
            //将Redis分布式缓存服务添加到服务中
            services.AddDistributedRedisCache(options =>
            {
                options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
                {
                    //Ssl = true, //设置Redis启用SSL安全加密传输数据。
                    //Password = "1qaz@WSX3edc$RFV",
                    ConnectTimeout = 5000,//设置建立连接到Redis服务器的超时时间为5000毫秒
                    SyncTimeout = 5000,//设置对Redis服务器进行同步操作的超时时间为5000毫秒
                    ResponseTimeout = 5000//设置对Redis服务器进行操作的响应超时时间为5000毫秒
                };
                if (!string.IsNullOrEmpty(passWord))
                    options.ConfigurationOptions.Password = passWord;
                options.ConfigurationOptions.EndPoints.Add(redisConfiguration);
                options.ConfigurationOptions.DefaultDatabase = defaultDb;
                if (!string.IsNullOrEmpty(alias))
                    options.InstanceName = alias;
            });
            //接口在前，实现在后
            services.AddSingleton<IRedisCacheManager, RedisCacheManager>();
            //支持redis五种数据类型，及相关
            services.AddSingleton(new CSRedisHelper());
        }

    }
}
