using System;
using System.Collections.Generic;
using System.Text;

namespace AL.Common.Base
{
    /// <summary>
    /// 基类实体
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// 是否无效 T有效  F无效
        /// </summary>       
        public string IsVaild { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int? CreateUser { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public int? ModifyUser { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }

    }
}
