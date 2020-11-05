using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using Autofac;
using System.Reflection;
using Autofac.Extras.DynamicProxy;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using AL.Common.API;
using AL.Common.API.Extensions;
using AL.Common.API.Filters;
using AL.Common.API.Middlewares;
using Microsoft.Extensions.FileProviders;
using AL.Common.Tools;
using Microsoft.AspNetCore.Http;
using AL.Common.Base;

namespace AL.User.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 获取用户上下文
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IPrincipalAccessor, PrincipalAccessor>();
            services.AddSingleton<IUserContext, UserContext>();
            #endregion

            //log4net日志注册
            services.AddSingleton(new AL.Common.Tools.LoggerHelper.Log4netHelper());
            //日志AOP注册   
            services.AddSingleton(new AL.Common.Tools.LoggerLock(Env.ContentRootPath));
            //注册要通过反射创建的组件
            var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location); //获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
            //代码是必需的 没有的话，MiniProfiler会抛出错误
            services.AddMemoryCache();
            //sqlsugar
            services.AddSqlsugarSetup();
            //cors 跨域
            services.AddCorsSetup();
            //Automapper
            services.AddAutoMapperSetup(new Services.AddMapperExample());
            //Redis 缓存
            services.AddRedisCacheSetup();
            //MiniProfiler 
            services.AddMiniProfilerSetup();
            // Swagger UI
            services.AddSwaggerSetup(basePath, "AL.WebApi.xml", "AL.User.DTO.Models.xml");
            //Jwt授权
            services.AddAuthorizationSetup();
            //CAP //暂时不用
            //services.AddCapSetup();
            //request 自动验证model
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddControllers(s =>
            {
                s.Filters.Add(typeof(GlobalExceptionFilter));
                s.Filters.Add(typeof(ValidationFilter));
                s.Filters.Add(typeof(CheckTokenFilter));
            });
        }

        /// <summary>
        /// Autofac注册，注意在Program.CreateHostBuilder，添加Autofac服务工厂
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Swagger
            app.UseSwaggerMildd(() => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("AL.User.WebApi.index.html"));
            // Cors 跨域
            app.UseCors("CustomCorsPolicy");
            // 使用静态文件
            app.UseStaticFiles();
            //跳转
            app.UseHttpsRedirection();
            //路由
            app.UseRouting();
            //认证
            app.UseAuthentication();
            //授权
            app.UseAuthorization();
            // 性能分析 https://blog.csdn.net/qq_22949043/article/details/86063910
            app.UseMiniProfiler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
