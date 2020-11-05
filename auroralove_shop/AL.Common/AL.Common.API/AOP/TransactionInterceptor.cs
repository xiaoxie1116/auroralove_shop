using Castle.DynamicProxy;
using AL.Common.Base;
using AL.Common.Tools.LoggerHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AL.Common.API.AOP
{
    /// <summary>
    /// 事务AOP
    /// </summary>
    public class TransactionInterceptor : IInterceptor
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionInterceptor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            //对当前方法的特性验证
            //如果需要验证
            if (method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(UseTransactionAttribute)) is UseTransactionAttribute)
            {
                try
                {
                    //Console.WriteLine($"Begin Transaction");
                    _unitOfWork.BeginTran();

                    invocation.Proceed();

                    // 异步获取异常，先执行
                    if (IsAsyncMethod(invocation.Method))
                    {
                        var result = invocation.ReturnValue;
                        if (result is Task)
                        {
                            Task.WaitAll(result as Task);
                        }
                    }
                    _unitOfWork.CommitTran();
                }
                catch (Exception e)
                {
                    _unitOfWork.RollbackTran();
                    Log4netHelper.Error($"事务执行失败！回滚事务！原因:{e.Message}");
                }
            }
            else
            {
                invocation.Proceed();//直接执行被拦截方法
            }

        }

        public static bool IsAsyncMethod(MethodInfo method)
        {
            return (
                method.ReturnType == typeof(Task) ||
                (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                );
        }
    }
}
