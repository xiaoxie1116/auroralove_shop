using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace AL.Common.Base
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ISqlSugarClient _sqlSugarClient;

        public UnitOfWork(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
        }

        /// <summary>
        /// 获取DB，保证唯一性
        /// </summary>
        /// <returns></returns>
        public SqlSugarClient CreateDbClient()
        {
            
            return _sqlSugarClient as SqlSugarClient;
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTran()
        {
            CreateDbClient().BeginTran();
        }

        /// <summary>
        /// 事务提交
        /// </summary>
        public void CommitTran()
        {
            try
            {
                CreateDbClient().CommitTran();
            }
            catch (Exception ex)
            {
                CreateDbClient().RollbackTran();
                throw ex;
            }
        }

        /// <summary>
        /// 事务回滚
        /// </summary>
        public void RollbackTran()
        {
            CreateDbClient().RollbackTran();
        }

    }
}
