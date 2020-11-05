using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using AL.Common.Tools.Util;

namespace AL.Common.Data.Enums.Order
{
    [Serializable, Description("订单管理状态"), DataContract]
    public enum OrderManagerListStatus
    {
        [Description("未知")]
        [EnumMember]
        Pending = 0,
        [Description("待采购")]
        [EnumMember]
        BePurchase = 1,
        [Description("待确认")]
        [EnumMember]
        BeConfirm = 2,
        [Description("已付定金")]
        [EnumMember]
        HasPayEarnest = 3,
        [Description("已付款")]
        [EnumMember]
        HasPaied = 4,
        [Description("待付全款")]
        [EnumMember]
        BePayAll = 5,
        [Description("退订中")]
        [EnumMember]
        Refounding = 6,
        [Description("已取消")]
        [EnumMember]
        Canceled = 7,
        [Description("已完成")]
        [EnumMember]
        Completed = 8


    }

    public class OrderManagerListStatusEnumsDict
    {
        public static readonly Dictionary<int, string> OrderManagerListStatusEnumDict;
        public static readonly Dictionary<string, string> OrderManagerListStatusFieldDict;
        static OrderManagerListStatusEnumsDict()
        {
            OrderManagerListStatusEnumDict = EnumUtil.GetEnumIntNameDict(OrderManagerListStatus.Pending);
            OrderManagerListStatusFieldDict = EnumUtil.GetEnumVarNameDict(OrderManagerListStatus.Pending);
        }
    }
}
