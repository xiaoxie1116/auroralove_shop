using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using IdentityModel;

namespace AL.Common.Tools.Helper
{
    /// <summary>
    /// Jwt 帮助类
    /// </summary>
    public class JwtHelper
    {
        /// <summary>
        /// 根据token解析用户信息
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static JwtTokenModel SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            if (jwtToken == null || jwtToken.Payload == null) return null;
            object value;
            JwtTokenModel userInfo = new JwtTokenModel();
            jwtToken.Payload.TryGetValue(JwtClaimTypes.Id, out value);
            userInfo.Id = value.ObjToInt();
            jwtToken.Payload.TryGetValue(JwtClaimTypes.Name, out value);
            userInfo.Name = value.ObjToString();
            jwtToken.Payload.TryGetValue(JwtClaimTypes.PhoneNumber, out value);
            userInfo.PhoneNumber = value.ObjToString();
            return userInfo;
        }


    }

    /// <summary>
    /// 令牌
    /// </summary>
    public class JwtTokenModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        public int? CompanyID { get; set; }

    }
}
