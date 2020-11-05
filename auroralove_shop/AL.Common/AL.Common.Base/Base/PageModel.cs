using System;
using System.Collections.Generic;
using System.Text;

namespace AL.Common.Base
{
    public class PageModel<T> where T : class
    {
        /// <summary>
        /// 当前页标
        /// </summary>
        public int page { get; set; } = 1;
        /// <summary>
        /// 总页数
        /// </summary>
        public int pageCount
        {
            get
            {
                int pageSize = PageSize > 0 ? PageSize : 20;
                return (dataCount + pageSize - 1) / pageSize;
            }
            set
            {
            }
        }
        /// <summary>
        /// 数据总数
        /// </summary>
        public int dataCount { get; set; } = 0;
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { set; get; } = 20;
        /// <summary>
        /// 返回数据
        /// </summary>
        public List<T> data { get; set; }

    }
}
