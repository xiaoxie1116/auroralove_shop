using AL.Common.Tools;
using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AL.Common.API.Middlewares
{
    /// <summary>
    /// 中间件扩展(针对webpai中的swagger)
    /// </summary>
    public static class ApiSwaggerMildd
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ApiSwaggerMildd));

        public static void UseSwaggerMildd(this IApplicationBuilder app, Func<Stream> streamHtml)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            var Enaled = Appsettings.GetSettingNode(new string[] { "Startup", "Swagger", "Enaled" }).ObjToBool();
            var VirtualPath = Appsettings.GetSettingNode(new string[] { "Startup", "Swagger", "VirtualPath" });
            if (Enaled && string.IsNullOrEmpty(VirtualPath))
                throw new Exception("虚拟路径不存在，请进行相关配置！");

            app.UseSwagger(c =>
            {
                //c.PreSerializeFilters.Add((a, b) => a.BasePath = new Microsoft.OpenApi.Models.OpenApiPaths() {  });     
                if (Enaled)
                {
                    c.RouteTemplate = $"/{VirtualPath}/swagger/{{documentName}}/swagger.json";
                    c.PreSerializeFilters.Add((doc, httpReq) =>
                    {
                        doc.Servers = new List<OpenApiServer> { new OpenApiServer {
                        Url = $"/{VirtualPath}"
                        //Url=$"{httpReq.Scheme}://{httpReq.Host.Value}/{VirtualPath}/"
                    }};
                    });
                }

            });

            app.UseSwaggerUI(c =>
            {
                //根据版本名称倒序 遍历展示
                var ApiName = Appsettings.GetSettingNode(new string[] { "Startup", "ApiName" });
                //c.SwaggerEndpoint("/swagger/v1/swagger.json", "HC.Core.NewSystem v1");

                if (Enaled)
                {
                    typeof(ApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                    {
                        c.SwaggerEndpoint($"{VirtualPath}/swagger/{version}/swagger.json", $"{ApiName} {version}");
                    });
                }
                else
                {
                    typeof(ApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                    {
                        c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{ApiName} {version}");
                    });
                }

                // 将swagger首页，设置成我们自定义的页面，记得这个字符串的写法：{项目名.index.html}
                if (streamHtml.Invoke() == null)
                {
                    var msg = "index.html的属性，必须设置为嵌入的资源";
                    log.Error(msg);
                    throw new Exception(msg);
                }
                c.IndexStream = streamHtml;
                c.RoutePrefix = "";
                // 路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，去launchSettings.json把launchUrl去掉，如果你想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "doc";
                //if (Enaled)
                //    c.RoutePrefix = $"{VirtualPath}/";
                //else
                //    
            });
        }


    }

    /// <summary>
    /// API 自定义版本
    /// </summary>
    public enum ApiVersions
    {
        v1 = 1
    }
}
