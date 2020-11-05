using AL.Common.Tools.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace AL.Common.Data.Enums
{
    [Serializable, Description("客户审核状态"), DataContract]
    public enum CustomerAuditStatus
    {
        [Description("待提交")]
        [EnumMember]
        ToSubmit,
        [Description("待审核")]
        [EnumMember]
        Review,
        [Description("审核中")]
        [EnumMember]
        Auditing,
        [Description("审核成功")]
        [EnumMember]
        Successed,
        [Description("驳回")]
        [EnumMember]
        Rejected
    }

    public class CustomerAuditStatusEnumsDict
    {
        public static readonly Dictionary<int, string> CustomerAuditStatusEnumDict;
        public static readonly Dictionary<string, string> CustomerAuditStatusEnumFieldDict;
        static CustomerAuditStatusEnumsDict()
        {
            CustomerAuditStatusEnumDict = EnumUtil.GetEnumIntNameDict(CustomerAuditStatus.ToSubmit);
            CustomerAuditStatusEnumFieldDict = EnumUtil.GetEnumVarNameDict(CustomerAuditStatus.ToSubmit);
        }
    }
}
