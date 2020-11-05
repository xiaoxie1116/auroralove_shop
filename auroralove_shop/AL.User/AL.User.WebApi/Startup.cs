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
            #region ��ȡ�û�������
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IPrincipalAccessor, PrincipalAccessor>();
            services.AddSingleton<IUserContext, UserContext>();
            #endregion

            //log4net��־ע��
            services.AddSingleton(new AL.Common.Tools.LoggerHelper.Log4netHelper());
            //��־AOPע��   
            services.AddSingleton(new AL.Common.Tools.LoggerLock(Env.ContentRootPath));
            //ע��Ҫͨ�����䴴�������
            var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location); //��ȡӦ�ó�������Ŀ¼�����ԣ����ܹ���Ŀ¼Ӱ�죬������ô˷�����ȡ·����
            //�����Ǳ���� û�еĻ���MiniProfiler���׳�����
            services.AddMemoryCache();
            //sqlsugar
            services.AddSqlsugarSetup();
            //cors ����
            services.AddCorsSetup();
            //Automapper
            services.AddAutoMapperSetup(new Services.AddMapperExample());
            //Redis ����
            services.AddRedisCacheSetup();
            //MiniProfiler 
            services.AddMiniProfilerSetup();
            // Swagger UI
            services.AddSwaggerSetup(basePath, "AL.WebApi.xml", "AL.User.DTO.Models.xml");
            //Jwt��Ȩ
            services.AddAuthorizationSetup();
            //CAP //��ʱ����
            //services.AddCapSetup();
            //request �Զ���֤model
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
        /// Autofacע�ᣬע����Program.CreateHostBuilder�����Autofac���񹤳�
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
            // Cors ����
            app.UseCors("CustomCorsPolicy");
            // ʹ�þ�̬�ļ�
            app.UseStaticFiles();
            //��ת
            app.UseHttpsRedirection();
            //·��
            app.UseRouting();
            //��֤
            app.UseAuthentication();
            //��Ȩ
            app.UseAuthorization();
            // ���ܷ��� https://blog.csdn.net/qq_22949043/article/details/86063910
            app.UseMiniProfiler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
