using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Extensions.DependencyInjection;
using Grpc.Core;
using AL.Common.API;
using AL.Common.API.Extensions;
using AL.Common.API.Middlewares;
using AL.Common.Base;
using AL.Common.Base.Repository;
using AL.Common.Base.Services;
using MagicOnion.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AL.User.WebApi.Grpc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        static string basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 获取用户上下文
            services.AddHttpContextAccessor();
            services.AddSingleton<IPrincipalAccessor, PrincipalAccessor>();
            services.AddSingleton<IUserContext, UserContext>();
            #endregion

            //注意：需要将所有的service类进行反射
            Assembly[] assemblyServices = new Assembly[] {
                //typeof(UserInfoServices).Assembly,
            };
            string modelXml = "AL.User.Grpc.Proxy.xml";
            var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            //Sqlsugar
            services.AddSqlsugarSetup(false);
            //添加所有的程序集服务注册
            services.AddAssemblyServices();
            //Automapper           
            services.AddAutoMapperSetup(new Services.AddMapperExample());
            //Redis 缓存
            services.AddRedisCacheSetup();
            //里面添加swagger的注册
            services.AddMagicOnionSetup(assemblyServices, basePath, modelXml, "UsersModule");
            //在Startup 服务配置中加入控制器替换规则   
            try
            {
                var providere = services.BuildServiceProvider();
                var engine = new GeneralEngine(providere);
                ExampleContext.Initialize(engine);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            services.AddControllers();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMagicOnionMildd(basePath, "AL.User.Grpc.Proxy.xml", "UsersModule");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API-v1");
                c.RoutePrefix = string.Empty;
            });
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
