using System;
using System.Collections.Generic;
using System.Text;

namespace AL.User.DTO.Models
{
    public class LoginInfo
    {
        /// <summary>
        /// token信息
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// token过期时间
        /// </summary>
        public long ExpireTime { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInformation UserInfo { get; set; }
    }

    public class UserInformation
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Int32 ID { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// 密码 MD5加密
        /// </summary>
        public String Pwd { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public String Phone { get; set; }

        /// <summary>
        /// Desc:生日
        /// </summary>           
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Desc:头像
        /// </summary>           
        public string HeadImg { get; set; }

        /// <summary>
        /// Desc:性别
        /// </summary>           
        public long? Sex { get; set; }

        /// <summary>
        /// Desc:所在城市
        /// </summary>           
        public int? FK_Citys { get; set; }

        /// <summary>
        /// Desc:所在店铺
        /// </summary>           
        public int? FK_Shop { get; set; }

        /// <summary>
        /// Desc:签名
        /// </summary>           
        public string Sign { get; set; }

        /// <summary>
        /// Desc:会员等级
        /// </summary>           
        public int? Level { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public int? RoleType { get; set; }
    }
}
