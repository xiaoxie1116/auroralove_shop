using System;
using System.Collections.Generic;
using System.Text;

namespace AL.Common.Data.Enums.Payment
{
   
     /// <summary>
     /// 交易状态
     /// </summary>
    public enum TradeStatus
    {
        /// <summary>
        /// 待交易
        /// </summary>
        WaitingTrade = 1,

        /// <summary>
        /// 成功
        /// </summary>
        Success = 2,

        /// <summary>
        /// 失败
        /// </summary>
        Fail = 3
    }
}
