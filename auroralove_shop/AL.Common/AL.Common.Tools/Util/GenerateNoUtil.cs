using System;
using System.Collections.Generic;
using System.Text;

namespace AL.Common.Tools.Util
{

    //有待改进
    public static class GenerateNoUtil
    {
        static object locker = new object();
        /**
        * 根据当前系统时间加随机序列来生成BillSeq的SerialNo（注意数据库长度）
        * @return 流水号
        */
        public static string GenerateBillSeqSerialNo(int userId=0, int orderId=0)
        {
            lock (locker)
            {
                return $"{userId}_{orderId}_{DateTime.Now.ToString("yyMMddHHmmss")}{new Random().Next(999)}";
                //return string.Format("{0}{1}-{2}", "", DateTime.Now.ToString("yyyyMMddHHmmss"), Guid.NewGuid()).Substring(2,30);
            }
        }
    }
}
