using ArxOne.MrAdvice.Advice;
using SD.AOP.Aspects.ForAny;
using System;

namespace SD.AOP.Tests.StubImplements
{
    /// <summary>
    /// 具体异常AOP特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class StubExceptionLogAspect : ExceptionAspect
    {
        #region Overrides of ExceptionAspect

        /// <summary>
        /// 发生异常事件
        /// </summary>
        /// <param name="context">方法元数据</param>
        /// <param name="exception">异常实例</param>
        protected override void OnException(MethodAdviceContext context, Exception exception)
        {
            base.OnException(context, exception);

            throw exception;
        }

        #endregion
    }
}
