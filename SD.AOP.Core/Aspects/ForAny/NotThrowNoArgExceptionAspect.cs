using PostSharp.Aspects;
using SD.AOP.Core.Toolkits;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace SD.AOP.Core.Aspects.ForAny
{
    /// <summary>
    /// 不抛出异常AOP特性类，
    /// 只记录日志（不记录方法参数）
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class NotThrowNoArgExceptionAspect : ExceptionAspect
    {
        /// <summary>
        /// 异常过滤器
        /// </summary>
        /// <param name="eventArgs">方法元数据</param>
        public override void OnException(MethodExecutionArgs eventArgs)
        {
            base._exceptionLog.BuildBasicInfo(eventArgs);
            base._exceptionLog.BuildExceptionInfo(eventArgs);

            //无需事务
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                //02.插入数据库
                Task.Run(() => LogWriter.Error(base._exceptionLog));

                scope.Complete();
            }
        }
    }
}