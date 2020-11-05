using Grpc.Core;
using AL.Common.Base.Base;
using MagicOnion.Server;
using MessagePack;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AL.Common.API.Extensions
{
    /// <summary>
    /// MagicOnion-grpc 启动
    /// 源代码：https://github.com/Cysharp/MagicOnion
    /// 案例： https://www.cnblogs.com/NMSLanX/p/8242105.html
    /// </summary>
    public static class MagicOnionSetup
    {
        /// <summary>
        /// 添加MagicOnion框架启动项
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblyServices">相关服务类的反射集合</param>
        /// <param name="basePath">根目录路径</param>
        /// <param name="modelsXml">XML注释名</param>     
        /// <param name="moduleName">XML注释名</param>  
        public static void AddMagicOnionSetup(this IServiceCollection services, Assembly[] assemblyServices, string basePath, string modelsXml, string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName)) throw new Exception("grpc模块的名称不能为空，请根据在全局配置文件中名称在启动项中进行配置！");
            string ipStr = Tools.LocalAppsetting.GetSettingNode(new string[] { "Startup", "Grpc", moduleName, "Ip" });
            string portStr = Tools.LocalAppsetting.GetSettingNode(new string[] { "Startup", "Grpc", moduleName, "Port" });

            if (assemblyServices.Length == 0) return;

            MagicOnionServiceFilterDescriptor filter = new MagicOnionServiceFilterDescriptor(new GlbFilter());

            // 通过反射去拿
            MagicOnionServiceDefinition service = MagicOnionEngine.BuildServerServiceDefinition(
                // 加载引用程序集             
                assemblyServices,
                new MagicOnionOptions(true)
                {
                    MagicOnionLogger = new MagicOnionLogToGrpcLogger(),
                    SerializerOptions = MessagePackSerializerOptions.Standard.WithResolver(
                           MessagePack.Resolvers.ContractlessStandardResolverAllowPrivate.Instance),
                    IsReturnExceptionStackTraceInErrorDetail = true,
                    GlobalFilters = new[] { filter },
                });

            if (string.IsNullOrEmpty(ipStr) || string.IsNullOrEmpty(portStr))
                throw new Exception("全局配置文件中 中没有对 grpc的ip或端口号没有进行相关的配置！");

            Server server = new Server
            {
                Services = { service },
                Ports = { new ServerPort(ipStr, int.Parse(portStr), ServerCredentials.Insecure) }
            };

            server.Start();
            //注册要通过反射创建的组件
            //var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            services.AddSwaggerGen(c =>
            {
                var filePath = Path.Combine(basePath, modelsXml);
                c.IncludeXmlComments(filePath);
            });

            //这里添加服务
            services.Add(new ServiceDescriptor(typeof(MagicOnionServiceDefinition), service));          
        }
    }


    public class GlbFilter : MagicOnionFilterAttribute
    {
        static List<string> WhiteIpList = new List<string>() { "[::1]", "127.0.0.1" };

        static GlbFilter()
        {
            var ips = Tools.LocalAppsetting.GetSettingNode("Startup", "Grpc", "WhiteIpList");
            var ipsArr = ips.Split(";");// ip之间用分号隔开
            foreach (var ip in ipsArr)
            {
                WhiteIpList.Add(ip.Trim());
            }
        }

        public override ValueTask Invoke(ServiceContext context, Func<ServiceContext, ValueTask> next)
        {
            var peer = context.CallContext.Peer;
            var ip = peer.Split(":")[1];
            if (WhiteIpList.Contains(ip))
            {
                return next(context);
            }

            context.SetRawResponse(Encoding.UTF8.GetBytes("403"));
            return new ValueTask();
        }
    }

}
