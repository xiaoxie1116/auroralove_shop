using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AL.Common.Data;
using AL.Common.Tools;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AL.Common.Base
{

    public interface IPrincipalAccessor
    {
        ClaimsPrincipal Principal { get; }
    }

    public class PrincipalAccessor : IPrincipalAccessor
    {
        public ClaimsPrincipal Principal => _httpContextAccessor.HttpContext?.User;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public PrincipalAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

    }

    /// <summary>
    /// 用户的上下文
    /// </summary>
    public interface IUserContext
    {
        UserInfo User { get; }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserContext : IUserContext
    {
        private readonly IPrincipalAccessor _principalAccessor;

        public UserContext(IPrincipalAccessor principalAccessor)
        {
            _principalAccessor = principalAccessor;
        }
        public UserInfo User
        {
            get
            {
                var user = new UserInfo();
                if (_principalAccessor != null)
                {
                    user.UserID = (_principalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Id)?.Value).ObjToInt();
                    user.UserName = _principalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name)?.Value;
                    user.Phone = _principalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.PhoneNumber)?.Value;
                    user.RoleType = (_principalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimConsts.RoleType)?.Value).ObjToInt();
                    user.JwtId = _principalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.JwtId)?.Value;
                    user.City = (_principalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimConsts.City)?.Value).ObjToInt();
                }
                return user;
            }
        }
    }

    public class UserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public int? RoleType { get; set; }

        /// <summary>
        /// token ID
        /// </summary>
        public string JwtId { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 所在城市
        /// </summary>
        public int City { get; set; }
    }

}
