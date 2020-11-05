using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AL.Common.Data.Enums.Payment
{
    /// <summary>
    /// ExceptionBillStatus
    /// </summary>
    public enum ExceptionBillStatus
    {
        /// <summary>
        /// 异常
        /// </summary>
        [Description("异常")]
        Exception = 1,

        /// <summary>
        /// 修复
        /// </summary>
        [Description("修复")]
        Fix = 2
    }
}
