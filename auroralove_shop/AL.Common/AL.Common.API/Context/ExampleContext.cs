using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace AL.Common.API
{
    /// <summary>
    /// 引擎上下文,在项目初始化过程中实例化一个引擎实例
    /// </summary>
    public class ExampleContext
    {
        private static IEngine _engine;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(IEngine engine)
        {
            if (_engine == null)
            {
                _engine = engine;
            }
            return _engine;
        }

        /// <summary>
        /// 当前引擎
        /// </summary>
        public static IEngine Current
        {
            get
            {
                return _engine;
            }
        }

    }
}
