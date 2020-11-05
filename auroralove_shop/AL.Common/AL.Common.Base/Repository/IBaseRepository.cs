﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AL.Common.Base.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> QueryById(object objId);

        Task<TEntity> QueryById(int objId);

        Task<TEntity> QueryById(object objId, bool blnUseCache = false);

        Task<TEntity> QueryById(int objId, bool blnUseCache = false);

        Task<List<TEntity>> QueryByIDs(object[] lstIds);

        Task<List<TEntity>> QueryByIDs(List<int> lstIds);

        Task<int> Add(TEntity model);

        Task<int> Adds(List<TEntity> models);

        Task<bool> DeleteById(object id);

        Task<bool> DeleteById(int id);

        Task<bool> Delete(TEntity model);

        Task<int> Delete(Expression<Func<TEntity, bool>> whereExpression);

        Task<bool> DeleteByIds(object[] ids);

        Task<bool> DeleteByIds(List<int> ids);

        Task<bool> Update(TEntity model);

        Task<bool> Update(List<TEntity> entitys);
        Task<bool> Update(List<TEntity> entitys, params string[] parames);
        Task<bool> Update(TEntity entity, string strWhere);

        Task<bool> Update(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string strWhere = "");

        Task<List<TEntity>> Query();

        Task<List<TEntity>> Query(string strWhere);

        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression);

        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string strOrderByFileds);

        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);

        Task<List<TEntity>> Query(string strWhere, string strOrderByFileds);

        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intTop, string strOrderByFileds);

        Task<List<TEntity>> Query(string strWhere, int intTop, string strOrderByFileds);

        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex, int intPageSize, string strOrderByFileds);

        Task<List<TEntity>> Query(string strWhere, int intPageIndex, int intPageSize, string strOrderByFileds);

        Task<PageModel<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null);

        Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new();

        Task<List<TEntity>> SqlQuery(string SqlName);

        Task<int> ExecuteProcedure(string ProcName, params KeyValues[] parames);

        Task<DataTable> ExecuteProcedureQuery(string ProcName, params KeyValues[] parames);

        Task<List<T>> QuerySql<T>(string sql, params SugarParameter[] paras) where T : class, new();
        Task<PageModel<T>> QueryPageSql<T>(string sql, int intPageIndex, int intPageSize, SugarParameter[] paras) where T : class, new();
    }
}