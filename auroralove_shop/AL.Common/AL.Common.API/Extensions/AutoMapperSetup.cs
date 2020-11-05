using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.Common.API.Extensions
{
    /// <summary>
    /// automapper 配置文件
    /// </summary>
    public static class AutoMapperSetup
    {
        public static void AddAutoMapperSetup<T>(this IServiceCollection services, T entity) where T : Profile
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(entity);
            }, AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
