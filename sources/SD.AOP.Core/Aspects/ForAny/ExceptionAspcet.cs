using PostSharp.Aspects;
using SD.AOP.Core.Attributes;
using SD.AOP.Core.Mediators;
using SD.AOP.Core.Models.Entities;
using SD.AOP.Core.Models.ValueObjects;
using SD.AOP.Core.Toolkits;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;

namespace SD.AOP.Core.Aspects.ForAny
{
    /// <summary>
    /// 异常AOP特性基类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public abstract class ExceptionAspect : OnExceptionAspect
    {
        /// <summary>
        /// 异常日志字段
        /// </summary>
        protected readonly ExceptionLog _exceptionLog;

        /// <summary>
        /// 异常消息
        /// </summary>
        protected ExceptionMessage _exceptionMessage;

        /// <summary>
        /// 构造器
        /// </summary>
        protected ExceptionAspect()
        {
            this._exceptionLog = new ExceptionLog();
        }

        /// <summary>
        /// 异常过滤器
        /// </summary>
        /// <param name="eventArgs">方法元数据</param>
        public override void OnException(MethodExecutionArgs eventArgs)
        {
            if (!eventArgs.Method.IsDefined(typeof(SkipExceptionAttribute)))
            {
                //初始化异常日志
                this._exceptionLog.BuildBasicInfo(eventArgs);
                this._exceptionLog.BuildMethodArgsInfo(eventArgs);
                this._exceptionLog.BuildExceptionInfo(eventArgs);

                //无需事务
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    //插入数据库
                    Task<Guid> newId = Task.Run(() => LogMediator.Write(this._exceptionLog));

                    scope.Complete();

                    //初始化异常消息
                    this._exceptionMessage = new ExceptionMessage(eventArgs.Exception.Message, newId.Result);
                }
            }
        }
    }
}