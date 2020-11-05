using AL.Common.Tools.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace AL.Common.Data.Enums.Users
{
    [Serializable, Description("合作类型"), DataContract]
    public enum CooperateType
    {
        [Description("未知")]
        [EnumMember]
        UnKnown = 0,
        [Description("公司")]
        [EnumMember]
        Company = 1,
        [Description("个人")]
        [EnumMember]
        Personal = 2
    }

    public class CooperateTypeEnumsDict
    {
        public static readonly Dictionary<int, string> CooperateTypeEnumDict;
        public static readonly Dictionary<string, string> CooperateTypeEnumFieldDict;
        static CooperateTypeEnumsDict()
        {
            CooperateTypeEnumDict = EnumUtil.GetEnumIntNameDict(CooperateType.UnKnown);
            CooperateTypeEnumFieldDict = EnumUtil.GetEnumVarNameDict(CooperateType.UnKnown);
        }
    }
}
