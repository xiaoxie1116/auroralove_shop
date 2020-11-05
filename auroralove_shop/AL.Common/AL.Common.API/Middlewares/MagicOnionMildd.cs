using MagicOnion;
using MagicOnion.Server;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MagicOnion.HttpGateway.Swagger;
using MagicOnion.HttpGateway.Swagger.Schemas;
using Grpc.Core;
namespace AL.Common.API.Middlewares
{
    public static class MagicOnionMildd
    {
        public static void UseMagicOnionMildd(this IApplicationBuilder app, string basePath, string modelXml, string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName)) throw new Exception("grpc模块的名称不能为空，请根据在全局配置文件中名称在启动项中进行配置！");
            string ipStr = Tools.LocalAppsetting.GetSettingNode(new string[] { "Startup", "Grpc", moduleName, "Ip" });
            string portStr = Tools.LocalAppsetting.GetSettingNode(new string[] { "Startup", "Grpc", moduleName, "Port" });

            if (string.IsNullOrEmpty(ipStr) || string.IsNullOrEmpty(portStr))
                throw new Exception("全局配置文件中 中没有对 grpc的ip或端口号没有进行相关的配置！");

            //获取添加了的服务
            var magicOnion = app.ApplicationServices.GetService<MagicOnionServiceDefinition>();
            //使用MagicOnion的Swagger扩展，就是让你的rpc接口也能在swagger页面上显示           
            //注意：swagger原生用法属性都是大写的，这里是小写。
            //var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            if (magicOnion == null) throw new Exception("未添加对应的services服务");

            var ApiName = Common.Tools.Appsettings.GetSettingNode(new string[] { "Startup", "ApiName" });
           
            app.UseMagicOnionSwagger(magicOnion.MethodHandlers, new SwaggerOptions("ApiCore-Grpc", $"华程-{ApiName}-grpc", "/")
            {
                Info = new Info()
                {
                    title = ApiName,
                    version = "v1",
                    description = "This is the API-Interface for MGrpc",
                    termsOfService = "by hc international tourism",
                    contact = new Contact
                    {
                        name = "hc",
                        email = ""
                    }
                },
                //使用Swagger生成的xml，就是你接口的注释
                XmlDocumentPath = Path.Combine(basePath, modelXml),//"HC.Core.Grpc.Proxy.xml"
            });

            //要想让rpc成为该web服务的接口，流量和协议被统一到你写的这个web项目中来，那么就要用个方法链接你和rpc
            //这个web项目承接你的请求，然后web去调用rpc获取结果，再返回给你。
            //因此需要下面这句话      
            app.UseMagicOnionHttpGateway(magicOnion.MethodHandlers, new Channel($"{ipStr}:{portStr}", ChannelCredentials.Insecure));
        }

    }
}
