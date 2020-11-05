using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace AL.Common.API
{
    /// <summary>
    /// 权限所需参数(自定义策略)
    /// 继承 IAuthorizationRequirement，用于设计自定义权限处理器PermissionHandler  
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(List<PermissionItem> permissions, string claimType, SigningCredentials signingCredentials)
        {
            Permissions = permissions;
            ClaimType = claimType;
            SigningCredentials = signingCredentials;
        }

        /// <summary>
        /// 用户权限集合
        /// </summary>
        public List<PermissionItem> Permissions { get; set; }

        /// <summary>
        /// 认证授权类型
        /// </summary>
        public string ClaimType { get; set; }

        /// <summary>
        /// 签名验证
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }
    }

    public class PermissionItem
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Url集合
        /// </summary>
        public List<string> ApiUrl { get; set; }
    }
}
