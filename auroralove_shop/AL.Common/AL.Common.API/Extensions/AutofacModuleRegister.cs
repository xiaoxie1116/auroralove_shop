using Autofac;
using Autofac.Extras.DynamicProxy;
using AL.Common.API.AOP;
using AL.Common.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AL.Common.API.Extensions
{
    /// <summary>
    /// autoFac模块的注入
    /// </summary>
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;
            //整个程序集的注入实现层级解耦，如果路径不对，请修改对应的生成路径
            string assemblyName = Appsettings.GetSettingNode(new string[] { "Startup", "AssemblyName" });
            var servicesDllFile = Path.Combine(basePath, $"{assemblyName}.Services.dll");
            var repositoryDllFile = Path.Combine(basePath, $"{assemblyName}.Repository.dll");

            if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
                throw new Exception("获取DI程序集文件路径不存在！");

            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            var cacheType = new List<Type>();

            //redis 缓存AOP
            if (Appsettings.GetSettingNode(new string[] { "AppSettings", "Redis", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<RedisInterceptor>();
                cacheType.Add(typeof(RedisInterceptor));
            }
            //日志AOP
            if (Appsettings.GetSettingNode(new string[] { "AppSettings", "LoggerAop", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<LoggerInterceptor>();
                cacheType.Add(typeof(LoggerInterceptor));
            }
            //事务AOP
            if (Appsettings.GetSettingNode(new string[] { "AppSettings", "TranAOP", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<TransactionInterceptor>();
                cacheType.Add(typeof(TransactionInterceptor));
            }

            builder.RegisterAssemblyTypes(assemblysServices)
                 .AsImplementedInterfaces() //表示注册的类型，以接口的方式注册不包括IDisposable接口
                 .InstancePerLifetimeScope() //即为每一个依赖或调用创建一个单一的共享的实例
                 .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;使用接口的拦截器，在使用特性 [Attribute] 注册时，注册拦截器可注册到接口(Interface)上或其实现类(Implement)上。使用注册到接口上方式，所有的实现类都能应用到拦截器。
                 .InterceptedBy(cacheType.ToArray());//将拦截器添加到要注入容器的接口或者类之上。(可以直接替换拦截器)

            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces().InstancePerDependency();

            //builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
            //builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
        }

    }
}
