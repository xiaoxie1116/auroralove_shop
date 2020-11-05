using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace AL.Common.Data.Enums.Order
{

    /// <summary>
    /// 只有预占、定金占位、担保占位、占位
    /// </summary>
    [Serializable, Description("订单状态"), DataContract]
    public enum OrderStatus
    {
        /// <summary>
        /// 未知的异常状态
        /// </summary>
        [Description("未知的异常状态")]
        [EnumMember]
        UnKnow = -1,

        /// <summary>
        /// 暂存/草稿
        /// </summary>
        [Description("暂存/草稿")]
        [EnumMember]
        Draft = 0,

        /// <summary>
        /// 预订
        /// </summary>
        [Description("预订")]
        [EnumMember]
        Reserve = 1,

        /// <summary>
        /// 预报
        /// </summary>
        [Description("预报")]
        [EnumMember]
        Forecast = 2,

        /// <summary>
        /// 预占
        /// </summary>
        [EnumMember]
        [Description("预占")]
        Camp_On = 3,

        /// <summary>
        /// 占位
        /// </summary>
        [EnumMember]
        [Description("占位")]
        Occupying = 4,

        /// <summary>
        /// 取消
        /// </summary>
        [Description("取消")]
        [EnumMember]
        Cancel = 5,

        /// <summary>
        /// 退团
        /// </summary>
        [Description("退团")]
        [EnumMember]
        CancelFromTour = 6,
        /// <summary>
        /// 清位
        /// </summary>
        [Description("清位")]
        [EnumMember]
        ClearBit = 7,

        /// <summary>
        /// 资金占位
        /// </summary>
        [Description("订金占位")]
        [EnumMember]
        OccupyingByEarnestMoney = 8,
        /// <summary>
        /// 资料占位
        /// </summary>
        [Description("资料占位")]
        [EnumMember]
        OccupyingByMaterial = 9,
        /// <summary>
        /// 担保占位
        /// </summary>
        [Description("担保占位")]
        [EnumMember]
        OccupyingByGuarantee = 10
    }
}
