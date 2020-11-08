using AL.Common.Base.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace AL.User.DTO.Models
{
    public class RoleListRequest : RequestPageBase
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
    }
}
