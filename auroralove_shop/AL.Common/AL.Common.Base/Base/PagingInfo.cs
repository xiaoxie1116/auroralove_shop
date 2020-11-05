using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AL.Common.Base.Base
{
    public class PagingInfoResponse
    {

        /// <summary>
        /// 符合查询条件的总记录数
        /// </summary>
        public int DataCount { get; set; }

        /// <summary>
        /// 符合查询条件的总页数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int CurPageIndex { get; set; }
    }

    public class PagingInfoQuery
    {
        public PagingInfoQuery()
        {
            SkipCount = 0;
            TakeCount = 20;
        }
        /// <summary>
        /// 本次希望跳过记录数
        /// </summary>
        public int SkipCount { get; set; }

        /// <summary>
        /// 本次希望获取记录数
        /// </summary>
        public int TakeCount { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int CurPageIndex { get; set; }

    }
}
