using AL.Common.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using AL.Common.Tools;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using IdentityModel;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization.Policy;

namespace AL.Common.API
{
    /// <summary>
    /// 权限授权处理器
    /// </summary>
    public class RoleAuthorizationPolicyHandler : AuthorizationHandler<PermissionRequirement>
    {
        IHttpContextAccessor _httpContextAccessor = null;
        ILogger<RoleAuthorizationPolicyHandler> _logger;
        public RoleAuthorizationPolicyHandler(IHttpContextAccessor httpContextAccessor, ILogger<RoleAuthorizationPolicyHandler> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            try
            {
                var data = await RedisHelper.HGetAllAsync<List<string>>(RedisConsts.Jurisdiction);
                if (data == null) throw new InvalidOperationException("当前无用户权限缓存信息，请先登录！");
                var list = (from item in data
                            orderby item.Key
                            select new PermissionItem
                            {
                                UserID = item.Key.ObjToInt(),
                                ApiUrl = item.Value.Select(c => c.ToLower()).ToList()
                            }).ToList();
                requirement.Permissions = list;
                var httpContext = _httpContextAccessor.HttpContext;
                //请求Url
                var questUrl = httpContext.Request.Path.Value.ToLower();
                //登录成功
                if (context.User != null && context.User.Claims.Any())
                {
                    var roleType = (context.User.Claims.SingleOrDefault(s => s.Type == ClaimConsts.RoleType)?.Value).ObjToInt();
                    //超级管理员不用检查权限
                    if (roleType != 1)
                    {
                        var userID = (context.User.Claims.SingleOrDefault(s => s.Type == JwtClaimTypes.Id)?.Value).ObjToInt();
                        var urls = requirement.Permissions.FirstOrDefault(c => c.UserID == userID)?.ApiUrl;                       
                        //权限中是否存在请求的url
                        if (urls != null && urls.Any() && urls.Contains(questUrl))
                        {
                            context.Succeed(requirement);
                        }
                        else
                        {
                            context.Fail();
                            return;
                        }
                    }
                }
                context.Succeed(requirement);
            }
            catch (Exception x)
            {
                _logger.LogError($"策略模式验证失败：{x.Message}");
            }
        }
    }
}
