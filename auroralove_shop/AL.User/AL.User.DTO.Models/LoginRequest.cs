using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AL.User.DTO.Models
{
    /// <summary>
    /// 用户登录
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage = "手机号不能为空")]
        public string Phone { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        public string Pwd { get; set; }

        /// <summary>
        /// 1 后台登录  2 app登录
        /// </summary>
        [Range(1, 2, ErrorMessage = "登录类型错误")]
        public int LoginType { get; set; }

    }
}
