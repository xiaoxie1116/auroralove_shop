using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AL.Common.Tools.DataConvert
{
    public static class MapperExtends
    {
        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static TTarget Map<TTarget>(this object source)
        {
            Type tsource = source.GetType();
            Type tdestination = typeof(TTarget);
            return GetOrCreateMapper(tsource, tdestination).Map<TTarget>(source);
        }

        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static TTarget Map<TTarget>(this IEnumerable source)
        {
            var a = typeof(TTarget);
            if (!typeof(TTarget).FullName.StartsWith("System."))
                throw new Exception("请输入list集合类型，不允许只输入对象类型！");
            Type tsource = source.GetType().GenericTypeArguments[0];
            Type tdestination = typeof(TTarget).GenericTypeArguments[0];
            return GetOrCreateMapper(tsource, tdestination).Map<TTarget>(source);
        }

        /// <summary>
        /// Gets the or create mapper.
        /// </summary>
        /// <param name="tsource">The tsource.</param>
        /// <param name="tdestination">The tdestination.</param>
        /// <returns></returns>
        static IMapper GetOrCreateMapper(Type tsource, Type tdestination)
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap(tsource, tdestination).ForAllMembers(opt => opt.Condition(srs => srs != null));
            }).CreateMapper();
        }
    }
}
