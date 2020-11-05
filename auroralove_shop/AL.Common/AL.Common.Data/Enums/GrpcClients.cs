using AL.Common.Tools.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace AL.Common.Data.Enums
{
    [Serializable, Description("grpc客户端"), DataContract]
    public enum GrpcClients
    {
        [Description("UsersModule")]
        [EnumMember]
        UsersModule,
        [Description("OrderModule")]
        [EnumMember]
        OrderModule,
        [Description("ProductsModule")]
        [EnumMember]
        ProductsModule,
        [Description("FinanceModule")]
        [EnumMember]
        FinanceModule,
        [Description("PaymentModule")]
        [EnumMember]
        PaymentModule
    }

    public class GrpcClientsEnumsDict
    {
        public static readonly Dictionary<int, string> GrpcClientsEnumDict;
        public static readonly Dictionary<string, string> GrpcClientsFieldDict;
        static GrpcClientsEnumsDict()
        {
            GrpcClientsEnumDict = EnumUtil.GetEnumIntNameDict(GrpcClients.UsersModule);
            GrpcClientsFieldDict = EnumUtil.GetEnumVarNameDict(GrpcClients.UsersModule);
        }

        /// <summary>
        /// 获取客户端描述
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetGrpcClientsStr(int status)
        {
            StringBuilder result = new StringBuilder();
            if (status > 0)
            {
                foreach (var item in GrpcClientsEnumDict)
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
                result.Append(GrpcClientsEnumDict[status]);
            }
            return result.ToString();
        }
    }
}
