using System;
using System.Collections.Generic;
using System.Text;

namespace AL.User.DTO.Models
{
    public class RoleListResponse
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdateTime { get; set; }

    }
}
