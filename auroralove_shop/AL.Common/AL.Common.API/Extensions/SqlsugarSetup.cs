using AL.Common.Base;
using AL.Common.Tools;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.Common.API.Extensions
{
    /// <summary>
    /// Sqlsugar 启动服务
    /// </summary>
    public static class SqlsugarSetup
    {
        private static string _connectionWriteStr;
        private static List<string> _connectionReadStr = new List<string>();

        /// <summary>
        /// sqlsugar 服务启动
        /// </summary>
        /// <param name="services"></param>
        /// <param name="useMiniProfiler">是否使用性能分析工具</param>
        public static void AddSqlsugarSetup(this IServiceCollection services, bool useMiniProfiler = true)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            _connectionWriteStr = LocalAppsetting.GetSettingNode(new string[] { "AppSettings", "MySql", "Write", "ConnectionString" });
            var connStr = LocalAppsetting.GetSettingNode(new string[] { "AppSettings", "MySql", "Read", "ConnectionString" });
            var DbName = Appsettings.GetSettingNode(new string[] { "AppSettings", "MySql", "DBName" });
            if (string.IsNullOrEmpty(_connectionWriteStr))
                throw new ArgumentNullException("数据库连接字符串为空,请在全局配置文件中进行相关配置！");
            if (string.IsNullOrEmpty(DbName))
                throw new ArgumentNullException("数据库名称为空，请在项目配置文件中进行相关配置！");
            try
            {
                _connectionWriteStr = string.Format(_connectionWriteStr, DbName);
                connStr = string.Format(connStr, DbName);
            }
            catch
            {
                throw new Exception("全局配置文件中，数据库中的连接字符串中的占位符配置错误，请重新配置！");
            }
            if (!string.IsNullOrEmpty(connStr))
            {
                if (connStr.Contains('|'))
                {
                    _connectionReadStr.AddRange(connStr.Split('|'));
                }
                else
                    _connectionReadStr.Add(connStr);
            }

            List<SlaveConnectionConfig> connectionConfigs = null;
            if (_connectionReadStr != null && _connectionReadStr.Any())
            {
                connectionConfigs = new List<SlaveConnectionConfig>();
                foreach (var item in _connectionReadStr)
                {
                    connectionConfigs.Add(new SlaveConnectionConfig
                    {
                        ConnectionString = item,
                        HitRate = 10
                    });
                }
            }

            bool reval = false;
            var isSqlAop = Appsettings.GetSettingNode(new string[] { "AppSettings", "SqlAOP", "Enabled" });
            reval = !string.IsNullOrEmpty(isSqlAop) ? bool.Parse(isSqlAop) : false;

            //把多个连接对象注入服务，这里必须采用Scope，因为有事务操作

            //数据库读写分离（自动将写的操作连接到主库，读的操作连接到次库）
            services.AddScoped<ISqlSugarClient>(s =>
            {
                // 连接字符串
                var listConfig = new List<ConnectionConfig>();
                return new SqlSugarClient(new ConnectionConfig()
                {
                    //主连接
                    ConnectionString = _connectionWriteStr,
                    DbType = DbType.MySql,
                    IsAutoCloseConnection = true,
                    IsShardSameThread = false,
                    //从连接
                    SlaveConnectionConfigs = connectionConfigs,
                    //如果是非正常的数据类型ORM不支持可以自已添加扩展，其他功能详见官网设置
                    ConfigureExternalServices = new ConfigureExternalServices() { },
                    //用于一些全局设置
                    MoreSettings = new ConnMoreSettings()
                    {
                        IsAutoRemoveDataCache = false  //为true表示可以自动删除二级缓存
                    },
                    AopEvents = new AopEvents
                    {
                        OnLogExecuting = (sql, p) =>
                        {
                            if (reval)
                            {
                                Parallel.For(0, 1, e =>
                                {
                                    if (useMiniProfiler)
                                        MiniProfiler.Current.CustomTiming("SQL：", GetParas(p) + "【SQL语句】：" + sql);
                                    LoggerLock.OutPutLogger("SqlLog", new string[] { GetParas(p), "【SQL语句】：" + sql });
                                });
                            }
                        }
                    }
                });

            });

            //因为这个类在Common.Base程序集中，加载程序集的顺序导致没有注册该实例对象，需要在这里注册       
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }


        private static string GetParas(SugarParameter[] pars)
        {
            string key = "【SQL参数】：";
            foreach (var param in pars)
            {
                key = string.Format("{0} {1}", key, $"{param.ParameterName}:{param.Value}\n");
            }
            return key;
        }

    }
}
