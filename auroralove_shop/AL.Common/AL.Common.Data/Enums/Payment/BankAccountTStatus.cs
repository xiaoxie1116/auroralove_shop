using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AL.Common.Data.Enums.Payment
{
    public enum BankAccountTStatus
    {
        /// <summary>
        /// 待审核
        /// </summary>
        [Description("待审核")]
        Pending = 1,

        /// <summary>
        /// 通过
        /// </summary>
        [Description("审核通过")]
        Pass=2,

        /// <summary>
        /// 拒绝
        /// </summary>
        [Description("审核拒绝")]
        Reject = 3
    }
}
