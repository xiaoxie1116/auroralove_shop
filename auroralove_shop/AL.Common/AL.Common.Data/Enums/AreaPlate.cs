using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace AL.Common.Data.Enums
{
    public enum AreaPlate
    {
        [Description("非特定")]
        [EnumMember]
        UN = 0,
        [Description("亚洲")]
        [EnumMember]
        YZ = 1,
        [Description("欧洲")]
        [EnumMember]
        OZ = 2,
        [Description("北美洲")]
        [EnumMember]
        BM = 3,
        [Description("南美洲")]
        [EnumMember]
        NM = 4,
        [Description("非洲")]
        [EnumMember]
        FZ = 5,
        [Description("大洋洲")]
        [EnumMember]
        DY = 6,
        [Description("南极洲")]
        [EnumMember]
        NJ = 7,
        [Description("中国")]
        [EnumMember]
        ZG = 12,
        [Description("台湾")]
        [EnumMember]
        TW = 8
       ,
        [Description("海岛")]
        [EnumMember]
        HD = 9
       ,
        [Description("日本")]
        [EnumMember]
        RB = 10
       ,
        [Description("韩国")]
        [EnumMember]
        HG = 11
    }
}
