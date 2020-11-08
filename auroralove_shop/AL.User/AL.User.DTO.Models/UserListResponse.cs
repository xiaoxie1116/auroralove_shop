using System;
using System.Collections.Generic;
using System.Text;

namespace AL.User.DTO.Models
{
    /// <summary>
    /// 用户列表
    /// </summary>
    public class UserListResponse
    {
        public int ID { get; set; }

        /// <summary>
        /// Desc:用户昵称
        /// </summary>           
        public string UserName { get; set; }

        /// <summary>
        /// Desc:电话号码
        /// </summary>           
        public string Phone { get; set; }

        /// <summary>
        /// 用户类型  1 B端用户  2 C端用户
        /// </summary>
        public short UserType { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Desc:生日
        /// </summary>           
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Desc:性别
        /// </summary>           
        public short? Sex { get; set; }

        /// <summary>
        /// Desc:会员等级
        /// </summary>           
        public int? Level { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime? LoginLastTime { get; set; }

        /// <summary>
        /// 所在城市
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 所在店铺
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// Desc:是否可用
        /// </summary>           
        public string IsValid { get; set; }
    }
}
