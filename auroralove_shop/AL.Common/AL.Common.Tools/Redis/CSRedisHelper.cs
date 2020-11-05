using CSRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AL.Common.Tools.Redis
{
    /// <summary>
    /// 使用CSredis框架，支持redis的五种数据类型，及发布订阅模式
    /// </summary>
    public class CSRedisHelper : RedisHelper
    {
        static CSRedisHelper()
        {
            string redisConfiguration = Appsettings.GetSettingNode(new string[] { "AppSettings", "Redis", "ConnectionString" });//获取连接字符串
            var csredis = new CSRedisClient(redisConfiguration);
            //初始化
            Initialization(csredis);
        }
    }
}
