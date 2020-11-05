using AL.Common.Tools.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace AL.Common.Data.Enums
{
    [Serializable, Description("客户公司审核状态"), DataContract]
    public enum CompanyAuditStatus
    {
        [Description("待处理")]
        [EnumMember]
        Pending = 1,
        [Description("已处理")]
        [EnumMember]
        Agree = 2,
        [Description("已拒绝")]
        [EnumMember]
        Refuse = 3
    }

    public class CompanyAuditStatusEnumsDict
    {
        public static readonly Dictionary<int, string> CompanyAuditStatusEnumDict;
        public static readonly Dictionary<string, string> CompanyAuditStatusFieldDict;
        static CompanyAuditStatusEnumsDict()
        {
            CompanyAuditStatusEnumDict = EnumUtil.GetEnumIntNameDict(CompanyAuditStatus.Pending);
            CompanyAuditStatusFieldDict = EnumUtil.GetEnumVarNameDict(CompanyAuditStatus.Pending);
        }
    }
}
