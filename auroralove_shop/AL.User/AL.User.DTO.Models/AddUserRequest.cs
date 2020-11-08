using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AL.User.DTO.Models
{
    public class AddOrUpdateUserModel : RegisterUser
    {
        public int UserID { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public int? RoleId { get; set; }

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
        public short? Sex { get; set; }

        /// <summary>
        /// Desc:所在城市
        /// </summary>           
        public int? CityId { get; set; }

        /// <summary>
        /// Desc:所在店铺
        /// </summary>           
        public int? ShopId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public string IsValid { get; set; }
    }

    public class RegisterUser
    {
        /// <summary>
        /// Desc:用户昵称
        /// </summary>           
        [Required(ErrorMessage = "用户昵称不能为空")]
        public string UserName { get; set; }

        /// <summary>
        /// Desc:密码
        /// </summary>           
        [Required(ErrorMessage = "密码不能为空")]
        public string Pwd { get; set; }

        /// <summary>
        /// Desc:电话号码
        /// </summary>           
        [Required(ErrorMessage = "电话号码不能为空")]
        public string Phone { get; set; }
    }
}
