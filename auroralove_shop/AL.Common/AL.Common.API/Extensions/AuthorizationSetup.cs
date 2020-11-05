using AL.Common.Data;
using AL.Common.Tools;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AL.Common.API.Extensions
{
    /// <summary>
    /// Jwt 认证授权服务
    /// </summary>
    public static class AuthorizationSetup
    {
        public static void AddAuthorizationSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            //参数的构建
            //获取密钥
            var secret = LocalAppsetting.GetSettingNode(new string[] { "Jwt", "Secret" });
            var keyByteArray = Encoding.ASCII.GetBytes(secret);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            //获取 颁发者和使用者（听众）
            var Issuer = LocalAppsetting.GetSettingNode(new string[] { "Jwt", "Issuer" });
            var Audience = LocalAppsetting.GetSettingNode(new string[] { "Jwt", "Audience" });

            // 令牌验证参数  //3+2模式
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                ValidateIssuer = true,
                ValidIssuer = Issuer,//发行人

                ValidateAudience = true,
                ValidAudience = Audience,//订阅人

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(60 * 2),
                RequireExpirationTime = true,
            };

            var permission = new List<PermissionItem>();
            var permissionRequirement = new PermissionRequirement(permission,
                ClaimTypes.Role, //基于用户角色对应的权限授权            
                signingCredentials//签名
                );

            services
            .AddAuthorization(options =>
             {
                 // 自定义基于策略的授权权限
                 options.AddPolicy("RoleAuth", policy => policy.Requirements.Add(permissionRequirement));
             })
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //要开启bearer赋值,否则authorize标签有问题
                //x.DefaultChallengeScheme = nameof(ApiResponseHandler);
                x.DefaultForbidScheme = nameof(ApiResponseHandler);
            })
            // 开启Bearer认证
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
                options.SaveToken = true;
            })
            .AddScheme<AuthenticationSchemeOptions, ApiResponseHandler>(nameof(ApiResponseHandler), o => { });
            // 将授权必需类注入生命周期内
            services.AddSingleton(permissionRequirement);
            // 依赖注入，将自定义的授权处理器 匹配给官方授权处理器接口，这样当系统处理授权的时候，就会直接访问我们自定义的授权处理器了。
            services.AddSingleton<IAuthorizationHandler, RoleAuthorizationPolicyHandler>();
        }
    }


}
