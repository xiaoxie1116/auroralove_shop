using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AL.Common.Tools.DataConvert
{
    /// <summary>
    /// 利用表达式目录树进行实体映射
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    public static class ExpressionMapper<TSource, TTarget>
        where TSource : class, new()
        where TTarget : class, new()
    {
        private static Func<TSource, TTarget> _func = null;

        static ExpressionMapper()
        {
            try
            {
                ParameterExpression parameterExpression = Expression.Parameter(typeof(TSource), "s");
                List<MemberBinding> memberBindings = new List<MemberBinding>();
                foreach (var item in typeof(TTarget).GetProperties())
                {
                    var sProp = typeof(TSource).GetProperty(item.Name);
                    if (sProp != null && sProp.PropertyType == item.PropertyType && sProp.CanWrite && item.CanWrite)
                    {
                        MemberExpression property = Expression.Property(parameterExpression, sProp);
                        MemberBinding memberBinding = Expression.Bind(item, property);
                        memberBindings.Add(memberBinding);
                    }
                }
                foreach (var item in typeof(TTarget).GetFields())
                {
                    var sField = typeof(TSource).GetField(item.Name);
                    if (sField != null && sField.FieldType == item.FieldType)
                    {
                        MemberExpression property = Expression.Field(parameterExpression, sField);
                        MemberBinding memberBinding = Expression.Bind(item, property);
                        memberBindings.Add(memberBinding);
                    }
                }
                MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TTarget)), memberBindings);

                Expression<Func<TSource, TTarget>> lamada = Expression.Lambda<Func<TSource, TTarget>>(memberInitExpression, new ParameterExpression[] {
            parameterExpression
            });
                _func = lamada.Compile();
            }
            catch (Exception ex)
            {
                LoggerHelper.Log4netHelper.Error($"{typeof(TSource).Name} 转 {typeof(TTarget).Name} 出错：{ex.Message}");
            }
        }

        public static TTarget Map(TSource source)
        {
            return _func.Invoke(source);
        }

        public static List<TTarget> Map(List<TSource> source)
        {
            var ret = new List<TTarget>();
            foreach (var item in source)
            {
                ret.Add(_func.Invoke(item));
            }
            return ret;
        }
    }
}