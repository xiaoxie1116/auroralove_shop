using AL.Common.Tools.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace AL.Common.Data.Enums
{
    /// <summary>
    /// 账单类型 AccountSeq
    /// </summary>
    [Serializable, Description("支付账单类型"), DataContract]
    public enum AccountChangeType
    {
        /// <summary>
        /// 消费
        /// </summary>
        [Description("消费")]
        Consume = 1,
        /// <summary>
        /// 充值
        /// </summary>
        [Description("充值")]
        Recharge,
        /// <summary>
        /// 提现
        /// </summary>
        [Description("提现")]
        CashOut,
        /// <summary>
        /// 提现退回
        /// </summary>
        [Description("提现退回")]
        CashReturn,
        /// <summary>
        /// 退款
        /// </summary>
        [Description("退款")]
        Refued
    }


    public class AccountChangeTypeEnumsDict
    {
        public static readonly Dictionary<int, string> AccountChangeTypeEnumDict;
        public static readonly Dictionary<string, string> AccountChangeTypeEnumFieldDict;
        static AccountChangeTypeEnumsDict()
        {
            AccountChangeTypeEnumDict = EnumUtil.GetEnumIntNameDict(AccountChangeType.Consume);
            AccountChangeTypeEnumFieldDict = EnumUtil.GetEnumVarNameDict(AccountChangeType.Consume);
        }
    }

    /// <summary>
    /// 账单类型
    /// </summary>
    public enum BillChangeType
    {
        /// <summary>
        /// 订单支付
        /// </summary>
        OrderPay = 1,
        /// <summary>
        /// 充值
        /// </summary>
        Recharge,
        /// <summary>
        /// 提现
        /// </summary>
        CashOut,
        /// <summary>
        /// 提现退回
        /// </summary>
        CashReturn,
        /// <summary>
        /// 退款
        /// </summary>
        Refued,
        /// <summary>
        /// 余额支付
        /// </summary>
        BalancePay
    }
}
