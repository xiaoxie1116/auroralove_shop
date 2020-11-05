using AL.Common.Base;
using AL.Common.Tools;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AL.Common.API.Filters.RateLimiter
{
    /// <summary>
    /// 限流 by wo
    /// </summary>
    public class RateLimitingAttirbute : ActionFilterAttribute
    {
        //private readonly IUserContext _userContext;
        //public RateLimitingAttirbute(IUserContext userContext)
        //{
        //    _userContext = userContext;
        //}

        private readonly int _count;
        public RateLimitingAttirbute(int count)
        {
            _count = count;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userID = context.HttpContext.User.Claims.SingleOrDefault(s => s.Type == JwtClaimTypes.Id)?.Value;
            var RateLimiterKey = $"RateLimiter:{userID}{context.HttpContext.Request.Path.Value.Replace("/", ":")}";
            if (RedisHelper.Exists(RateLimiterKey))
            {
                string redisResult = RedisHelper.GetAsync(RateLimiterKey).Result;
                if (redisResult.ObjToInt() >= _count)
                {

                    context.Result = new JsonResult(new ResponseModel<bool>
                    {
                        success = false,
                        status = 400,
                        msg = "请求过于频繁！",
                    });
                }
                    
                else
                    RedisHelper.IncrByAsync(RateLimiterKey, 1);
            }
            else
            {
                //1分钟内限制count次请求
                RedisHelper.SetAsync(RateLimiterKey, 1, new TimeSpan(0, 0, 60));
            }
        }
    }
}
