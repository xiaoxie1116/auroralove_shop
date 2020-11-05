using System;
using System.Collections.Generic;
using System.Text;

namespace AL.Common.Base.Base
{
    /// <summary>
    /// 分页数据
    /// </summary>
    public class RequestPageBase
    {
        /// <summary>
        /// 页数
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 条数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 排序 （如 字段+排序关键字 ：" id desc "） orderby不用写
        /// </summary>
        public string OrderByStr { get; set; }

    }
}
