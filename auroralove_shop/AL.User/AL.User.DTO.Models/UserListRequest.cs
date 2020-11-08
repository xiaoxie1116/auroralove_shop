using AL.Common.Base.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace AL.User.DTO.Models
{
    public class UserListRequest : RequestPageBase
    {
        /// <summary>
        /// 用户名或者手机号
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所在店铺
        /// </summary>
        public int Shop { get; set; }
    }
}
