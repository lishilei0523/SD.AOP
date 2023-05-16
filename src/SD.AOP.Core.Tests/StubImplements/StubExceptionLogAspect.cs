using ArxOne.MrAdvice.Advice;
using SD.AOP.Core.Aspects.ForAny;
using System;

namespace SD.AOP.Core.Tests.StubImplements
{
    /// <summary>
    /// 具体异常AOP特性
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class StubExceptionLogAspect : ExceptionAspect
    {
        /// <summary>
        /// 发生异常事件
        /// </summary>
        protected override void OnException(MethodAdviceContext context, Exception exception)
        {
            base.OnException(context, exception);

            throw exception;
        }
    }
}
