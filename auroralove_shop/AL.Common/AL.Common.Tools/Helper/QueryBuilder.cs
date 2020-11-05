using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AL.Common.Tools.Helper
{
    public static class FilterQueryBuilder
    {
        public static IQueryBuilder<T> Create<T>()
        {
            return new FilterQueryBuilder<T>();

        }
    }

    class FilterQueryBuilder<T> : IQueryBuilder<T>
    {
        private Expression<Func<T, bool>> predicate;

        Expression<Func<T, bool>> IQueryBuilder<T>.Expression
        {

            get
            {
                return predicate;
            }
            set
            {
                predicate = value;
            }
        }

        public FilterQueryBuilder()
        {
            predicate = ExpressionHelper.True<T>();
        }
    }

    /// <summary>
    /// 动态查询条件创建者
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQueryBuilder<T>
    {
        Expression<Func<T, bool>> Expression { get; set; }
    }

    public static class IQueryBuilderExtensions
    {

        /// <summary>
        /// 建立 Between 查询条件
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="q">动态查询条件创建者</param>
        /// <param name="property">属性</param>
        /// <param name="from">开始值</param>
        /// <param name="to">结束值</param>
        /// <returns></returns>
        public static IQueryBuilder<T> Between<T, P>(this IQueryBuilder<T> q, Expression<Func<T, P>> property, P from, P to)
        {
            var parameter = property.GetParameters();
            //创建一个表示常量的表达式：datetime x = "2019-08-20"
            var constantFrom = Expression.Constant(from);
            var constantTo = Expression.Constant(to);
            Type type = typeof(P);

            //获取Lambda的方法体所表示的表达式
            Expression nonNullProperty = property.Body;

            //如果是Nullable<X>类型，则转化成X类型
            if (IsNullableType(type))
            {
                type = GetNonNullableType(type);
                nonNullProperty = Expression.Convert(property.Body, type);
            }

            //创建一个BinaryExpression表示>=的表达式 类似的有GreaterThan ()
            var c1 = Expression.GreaterThanOrEqual(nonNullProperty, constantFrom);
            //建一个BinaryExpression表示<=的表达式，类似的有LessThan()
            var c2 = Expression.LessThanOrEqual(nonNullProperty, constantTo);
            //表达式的拼接
            var c = Expression.AndAlso(c1, c2);
            //创建一个Expression<TDelagate>实例表示Lambda表达式，与非泛型版本的Lambda()方法相比，此方法显示指定了与Lambda对应的委托，执行Lambda时输入、输出参数会一目了然
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(c, parameter);
            q.Expression = q.Expression.And(lambda);
            return q;
        }


        /// <summary>
        /// 表达式中添加 lambda
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q"></param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public static IQueryBuilder<T> Add<T>(this IQueryBuilder<T> q, Expression<Func<T, bool>> lambda)
        {
            q.Expression = q.Expression.And(lambda);
            return q;
        }


        /// <summary>
        /// 建立 Equals(相等)查询条件
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="q">动态查询条件创建者</param>
        /// <param name="property">属性</param>
        /// <param name="value">查询值</param>
        /// <returns></returns>
        public static IQueryBuilder<T> Equals<T, P>(this IQueryBuilder<T> q, Expression<Func<T, P>> property, P value)
        {

            var parameter = property.GetParameters();
            var constant = Expression.Constant(value);
            Type type = typeof(P);
            Expression nonNullProperty = property.Body;

            //如果是Nullable<X>类型，则转化成X类型
            if (IsNullableType(type))
            {
                type = GetNonNullableType(type);
                nonNullProperty = Expression.Convert(property.Body, type);
            }
            var methodExp = Expression.Equal(nonNullProperty, constant);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(methodExp, parameter);
            q.Expression = q.Expression.And(lambda);
            return q;

        }


        /// <summary>
        /// 建立 Like ( 模糊 ) 查询条件 (此方法有问题，可以用Add方法)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="q">动态查询条件创建者</param>
        /// <param name="property">属性</param>
        /// <param name="value">查询值</param>
        /// <returns></returns>
        public static IQueryBuilder<T> Like<T>(this IQueryBuilder<T> q, Expression<Func<T, string>> property, string value)
        {
            value = value.Trim();
            if (!string.IsNullOrEmpty(value))
            {
                var parameter = property.GetParameters();
                var constant = Expression.Constant("%" + value + "%");
                Expression nonNullProperty = property.Body;
                if (nonNullProperty != null)
                {
                    //Call() 方法
                    //创建一个MethodCallExpression表示调用某个方法的表达式，只有表示方法调用的表达式和块表达式可以执行
                    //方法调用有两种情况：1.对象调用方法 2.类型调用方法 比如：Animal a=new Animal(); a.Show()区别于Animal.Count()
                    //如果不是对象调用方法则第一个参数可提供null，否则第一个参数需要提供调用方法的对象，对象也必须是一个Expression         
                    MethodCallExpression methodExp = Expression.Call(
                        null, // 无实例调用方法
                        typeof(String).GetMethod("Contains", new Type[] { typeof(string), typeof(string) }), //方法调用的表达式
                        property.Body, constant  //方法的参数
                        );
                    Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(methodExp, parameter);
                    q.Expression = q.Expression.And(lambda);
                }

            }
            return q;
        }


        /// <summary>
        /// 建立 In 查询条件 (此方法有问题，可以用Add方法)
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="q">动态查询条件创建者</param>
        /// <param name="property">属性</param>
        /// <param name="valuse">查询值</param>
        /// <returns></returns>
        public static IQueryBuilder<T> In<T, P>(this IQueryBuilder<T> q, Expression<Func<T, P>> property, params P[] values)
        {
            if (values != null && values.Length > 0)
            {
                var parameter = property.GetParameters();
                var constant = Expression.Constant(values);
                Type type = typeof(P);
                Expression nonNullProperty = property.Body;

                //如果是Nullable<X>类型，则转化成X类型
                if (IsNullableType(type))
                {
                    type = GetNonNullableType(type);
                    nonNullProperty = Expression.Convert(property.Body, type);
                }
                Expression<Func<P[], P, bool>> InExpression = (list, el) => list.Contains(el);
                var methodExp = InExpression;
                //创建一个InvocationExpression表示执行Lambda并传递实参的表达式
                var invoke = Expression.Invoke(methodExp, //函数
                    constant, //参数 list
                    property.Body //参数 el
                    );
                Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(invoke, parameter);
                q.Expression = q.Expression.And(lambda);
            }
            return q;
        }

        private static ParameterExpression[] GetParameters<T, S>(this Expression<Func<T, S>> expr)
        {
            return expr.Parameters.ToArray();
        }


        static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        static Type GetNonNullableType(Type type)
        {
            return type.GetGenericArguments()[0];
        }

    }
}
