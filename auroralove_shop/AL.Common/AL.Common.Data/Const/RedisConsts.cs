using System;
using System.Collections.Generic;
using System.Text;

namespace AL.Common.Data
{
    /// <summary>
    /// redis 缓存的常量名称
    /// </summary>
    public class RedisConsts
    {
        // set 类型 key 为用户ID ,value 为 list<string> 用户可以访问的所有api接口
        public const string Jurisdiction = "Jurisdiction";

        // set 类型 key 为用户ID, value 为list<string> 该用户所有的token集合，及对应的guid（登录）
        public const string UserToken = "UserToken";

    }
}
