using System;
using System.Collections.Generic;
using System.Text;

namespace AL.Common.API
{
    /// <summary>
    /// 一个负责创建对象的引擎
    /// </summary>
    public interface IEngine
    {
        T Resolve<T>();
    }
}
