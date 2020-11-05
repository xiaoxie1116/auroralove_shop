using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AL.Common.Tools;
using System.IO;
using System.Linq;

namespace AL.Common.API
{
    /// <summary>
    /// DI 扩展方法--实现程序集批量注册
    /// </summary>
    public static class DIRegisterSetup
    {
        public static IServiceCollection AddAssemblyServices(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            var basePath = AppContext.BaseDirectory;
            string assemblyName = Appsettings.GetSettingNode(new string[] { "Startup", "AssemblyName" });
            if (string.IsNullOrEmpty(assemblyName))
                throw new ArgumentNullException("配置节点中缺少AssemblyName程序集配置节点，请完善！");
            var servicesDllFile = Path.Combine(basePath, $"{assemblyName}.Services.dll");
            var repositoryDllFile = Path.Combine(basePath, $"{assemblyName}.Repository.dll");
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            TypeAassemblyes(services, assemblysRepository);
            TypeAassemblyes(services, assemblysServices);
            return services;
        }

        public static void TypeAassemblyes(IServiceCollection services, Assembly assembly, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            try
            {
                var typeList = new List<Type>();//所有符合注册条件的类集合

                //筛选当前程序集下符合条件的类
                List<Type> types = assembly.GetTypes()
                   .Where(t => t.IsClass && !t.IsGenericType)//排除了泛型类          
                   .Where(t => t.Name.EndsWith("Repository") || t.Name.EndsWith("Services")) //过滤其他的非服务
                   .ToList();

                typeList.AddRange(types);
                if (!typeList.Any()) return;

                var typeDic = new Dictionary<Type, Type[]>(); //待注册集合<class,interface>
                foreach (var type in typeList)
                {
                    var interfaces = type.GetInterfaces();   //获取接口
                    typeDic.Add(type, interfaces);
                }
                //循环实现类
                foreach (var instanceType in typeDic.Keys)
                {
                    Type[] interfaceTypeList = typeDic[instanceType].Where(it => it.Name == $"I{instanceType.Name}").ToArray();
                    var descriptor = new ServiceDescriptor(interfaceTypeList?.FirstOrDefault() ?? instanceType, instanceType, serviceLifetime);
                    services.Add(descriptor);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }




    }
}
