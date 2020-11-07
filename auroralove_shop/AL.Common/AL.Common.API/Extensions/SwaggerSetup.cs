using AL.Common.API.Middlewares;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AL.Common.API.Extensions
{
    /// <summary>
    /// 设置 SwaggerUI 组件
    /// </summary>
    public static class SwaggerSetup
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Filters.GlobalExceptionFilter));
        public static void AddSwaggerSetup(this IServiceCollection services, string basePath, string webApiXml, string ModelXml)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var ApiName = Tools.Appsettings.GetSettingNode(new string[] { "Startup", "ApiName" });
            var Name = Tools.Appsettings.GetSettingNode(new string[] { "Startup", "Name" });
            var url = Tools.Appsettings.GetSettingNode(new string[] { "Startup", "WebSiteUrl" });
            services.AddSwaggerGen(c =>
            {
                //遍历出全部的版本，做文档信息展示
                typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version,
                        Title = $"{ApiName} 接口文档——{RuntimeInformation.FrameworkDescription}",
                        Description = Name + version,
                        Contact = new OpenApiContact { Name = ApiName, Email = "", Url = new Uri(url) },
                    });
                    c.OrderActionsBy(o => o.RelativePath);
                });

                try
                {
                    // 为 Swagger JSON and UI设置xml文档注释路径
                    var xmlPath = Path.Combine(basePath, webApiXml);//"HC.NewSystem.WebApi.xml"
                    c.IncludeXmlComments(xmlPath, true);
                    var xmlModelPath = Path.Combine(basePath, ModelXml); // "HC.Core.DTO.Models.xml"   Model实体注释                  
                    c.IncludeXmlComments(xmlModelPath);
                }
                catch (Exception ex)
                {
                    log.Error("swagger UI所需 xml 文件不存在，请检查\n" + ex.Message);
                }

                // 开启加权小锁
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                // 在header中添加token，传递到后台
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                // Jwt Bearer 认证，必须是 oauth2
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "请直接在下框中输入请求头中需要添加的授权：Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });

            });

        }
    }
}
