using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.Common.API.Extensions
{
    /// <summary>
    /// MiniProfiler 启动服务
    /// </summary>
    public static class MiniProfilerSetup
    {
        public static void AddMiniProfilerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
         
            // 3.x使用MiniProfiler，必须要注册MemoryCache服务
            services.AddMiniProfiler(options =>
            {
                //显示位置
                options.PopupRenderPosition = StackExchange.Profiling.RenderPosition.Left;
                //设定在弹出的明细窗口里会显式Time With Children这列
                options.PopupShowTimeWithChildren = true;
                options.RouteBasePath = "/profiler";
            });
        }
    }
}
