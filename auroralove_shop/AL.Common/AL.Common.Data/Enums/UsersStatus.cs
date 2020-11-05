using AL.Common.Tools.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace AL.Common.Data.Enums
{
    [Serializable, Description("客户审核状态，二进制取法"), DataContract]
    public enum UsersStatus
    {
        [Description("正常")]
        [EnumMember]
        Normal = 1,
        [Description("停用")]
        [EnumMember]
        Stop = 2,
        [Description("冻结")]
        [EnumMember]
        Freeze = 4
    }

    public class UsersStatusEnumsDict
    {
        public static readonly Dictionary<int, string> UsersStatusEnumDict;
        public static readonly Dictionary<string, string> UsersStatusFieldDict;
        static UsersStatusEnumsDict()
        {
            UsersStatusEnumDict = EnumUtil.GetEnumIntNameDict(UsersStatus.Normal);
            UsersStatusFieldDict = EnumUtil.GetEnumVarNameDict(UsersStatus.Normal);
        }

        /// <summary>
        /// 获取用户状态Str
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetUsersStatusStr(int status)
        {
            StringBuilder result = new StringBuilder();
            if (status > 0)
            {
                foreach (var item in UsersStatusEnumDict)
                {
                    if (item.Key > 0 && (status & item.Key) == item.Key)
                    {
                        if (status > 1 && item.Key == 1)
                        {
                            continue;
                        }
                        result.Append(item.Value);
                        result.Append(" ");
                    }
                }
            }
            else
            {
                result.Append(UsersStatusEnumDict[status]);
            }
            return result.ToString();
        }

        /// <summary>
        /// 获取用户状态列表
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static List<int> GetUsersStatusList(int status)
        {
            var result = new List<int>();
            if (status > 0)
            {
                foreach (var item in UsersStatusEnumDict)
                {
                    if (item.Key > 0 && (status & item.Key) == item.Key)
                    {
                        if (status > 1 && item.Key == 1)
                        {
                            continue;
                        }
                        result.Add(item.Key);
                    }
                }
            }
            return result;
        }
    }
}
