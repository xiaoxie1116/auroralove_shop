using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace AL.Common.Base
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// 创建 sqlsugar client 实例
        /// </summary>
        /// <returns></returns>
        SqlSugarClient CreateDbClient();

        /// <summary>
        /// 开始事务
        /// </summary>
        void BeginTran();

        /// <summary>
        ///  提交事务
        /// </summary>
        void CommitTran();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollbackTran();
    }
}
