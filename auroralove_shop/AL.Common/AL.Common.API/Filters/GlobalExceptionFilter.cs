using AL.Common.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.Common.API.Filters
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(IWebHostEnvironment env, ILogger<GlobalExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var json = new ResponseModel<bool>();
            //错误信息
            json.msg = context.Exception.Message;
            var errorAudit = "Unable to resolve service for";
            if (!string.IsNullOrEmpty(json.msg) && json.msg.Contains(errorAudit))
                json.msg = json.msg.Replace(errorAudit, $"（若新添加服务，需要重新编译项目）{errorAudit}");
            //堆栈信息
            if (_env.IsDevelopment())
                json.msg = context.Exception.StackTrace;
            //定义500状态码
            context.Result = new InternalServerErrorObjectResult(json);
            MiniProfiler.Current.CustomTiming("Errors：", json.msg);
            //采用log4net 进行错误日志记录
            _logger.LogError(WriteLog(context.Exception));
        }

        /// <summary>
        /// 自定义返回格式
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public string WriteLog(Exception ex)
        {
            return $"\r\n 【异常类型】：{ex.GetType().Name} \t  \r\n【异常信息】：{ex.Message} \t \r\n 【堆栈信息】：{ex.StackTrace}";
        }

        /// <summary>
        /// 状态码为 500 服务器错误
        /// </summary>
        public class InternalServerErrorObjectResult : ObjectResult
        {
            public InternalServerErrorObjectResult(object value) : base(value)
            {
                StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}
