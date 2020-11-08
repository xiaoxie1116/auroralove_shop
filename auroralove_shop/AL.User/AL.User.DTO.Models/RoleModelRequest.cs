using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AL.User.DTO.Models
{
    public class RoleModelRequest
    {
        /// <summary>
        /// id 新增不需要赋值
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [Required(ErrorMessage = "角色名称不能为空")]
        [StringLength(32, ErrorMessage = "角色名称不能超过32个字符")]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(256, ErrorMessage = "角色描述不能超过256个字符")]
        public string Remark { get; set; }
    }
}
