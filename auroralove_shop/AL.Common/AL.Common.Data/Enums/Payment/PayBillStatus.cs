using AL.Common.Tools.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace AL.Common.Data.Enums
{
    /// <summary>
    /// 支付订单的状态
    /// </summary>
    [Serializable, Description("支付订单的状态"), DataContract]
    public enum PayBillStatus
    {
        /// <summary>
        /// 待支付
        /// </summary>
        [Description("待支付")]
        ToPay = 10,

        /// <summary>
        /// 支付成功
        /// </summary>
        [Description("支付成功")]
        SuccessPay = 20,

        /// <summary>
        /// 支付失败
        /// </summary>
        [Description("支付失败")]
        FailPay = 30,

        /// <summary>
        /// 受理中
        /// </summary>
        [Description("受理中")]
        Accepting = 40,

        /// <summary>
        /// 提现成功
        /// </summary>
        [Description("提现成功")]
        Withdrawal = 50,

        /// <summary>
        ///  提现失败
        /// </summary>
        [Description("提现失败")]
        WithdrawalFail = 60
    }

    public class PayBillStatusEnumsDict
    {
        public static readonly Dictionary<int, string> PayBillStatusEnumDict;
        public static readonly Dictionary<string, string> PayBillStatusEnumFieldDict;
        static PayBillStatusEnumsDict()
        {
            PayBillStatusEnumDict = EnumUtil.GetEnumIntNameDict(PayBillStatus.ToPay);
            PayBillStatusEnumFieldDict = EnumUtil.GetEnumVarNameDict(PayBillStatus.ToPay);
        }
    }


    [Serializable, Description("充值账单类型"), DataContract]
    public enum RechargeRecordStatus
    {
        /// <summary>
        /// 待支付
        /// </summary>
        [Description("等待充值")]
        ToPay = 10,

        /// <summary>
        /// 支付成功
        /// </summary>
        [Description("充值成功")]
        SuccessPay = 20,

        /// <summary>
        /// 支付失败
        /// </summary>
        [Description("充值关闭")]
        FailPay = 30,

    }

    public class RechargeRecordStatusEnumsDict
    {
        public static readonly Dictionary<int, string> RechargeRecordStatusEnumDict;
        public static readonly Dictionary<string, string> RechargeRecordStatusEnumFieldDict;
        static RechargeRecordStatusEnumsDict()
        {
            RechargeRecordStatusEnumDict = EnumUtil.GetEnumIntNameDict(RechargeRecordStatus.ToPay);
            RechargeRecordStatusEnumFieldDict = EnumUtil.GetEnumVarNameDict(RechargeRecordStatus.ToPay);
        }
    }



}
