using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AL.Common.Data.Enums.Payment
{
    /// <summary>
    /// 支付类型
    /// </summary>
    public enum PayType
    {
        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("支付宝")]
        Alipay = 1,

        /// <summary>
        /// 微信支付
        /// </summary>
        [Description("微信支付")]
        WeiXinPay =2
    }
}
