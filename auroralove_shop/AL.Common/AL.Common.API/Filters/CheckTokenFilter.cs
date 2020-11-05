using AL.Common.Data;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AL.Common.API.Filters
{
    /// <summary>
    ///  检查用户token
    /// </summary>
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CheckTokenFilter : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User == null || context.HttpContext.User.Claims == null || !context.HttpContext.User.Claims.Any()) return;
            //校验用户的token是否与和缓存中的一致 (返回ChallengeResult)
            var guid = context.HttpContext.User.Claims.SingleOrDefault(s => s.Type == JwtClaimTypes.JwtId)?.Value;
            var userID = context.HttpContext.User.Claims.SingleOrDefault(s => s.Type == JwtClaimTypes.Id)?.Value;
            var tokens = await RedisHelper.GetAsync<string>($"{RedisConsts.UserToken}:{userID}:{guid}");          
            if (tokens == null)
            {
                //先检查控制上的特性标签
                var conDescriptors = (context.ActionDescriptor as ControllerActionDescriptor)?.ControllerTypeInfo.GetCustomAttributes();
                if (conDescriptors != null)
                {
                    foreach (var conDescriptor in conDescriptors)
                    {
                        // 只检查有权限标签的
                        if (conDescriptor is AuthorizeAttribute)
                        {
                            context.Result = new ChallengeResult();
                            return;
                        }
                    }
                }
                //获取方法上的特性标签（）
                var descriptors = (context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo.GetCustomAttributes();
                if (descriptors != null)
                {                  
                    foreach (var descriptor in descriptors)
                    {
                        // 只检查有权限标签的
                        if (descriptor is AuthorizeAttribute)
                        {                          
                            context.Result = new ChallengeResult();
                            return;
                        }
                    }
                }
            }
        }
    }
}
