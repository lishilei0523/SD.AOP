using ArxOne.MrAdvice.Advice;
using SD.Toolkits.Json;
using System;

namespace SD.AOP.Core.Aspects.ForAny
{
    /// <summary>
    /// 默认异常AOP特性
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class DefaultExceptionAspect : ExceptionAspect
    {
        /// <summary>
        /// 发生异常事件
        /// </summary>
        /// <param name="context">方法元数据</param>
        /// <param name="exception">异常实例</param>
        protected override void OnException(MethodAdviceContext context, Exception exception)
        {
            base.OnException(context, exception);

            //抛出异常
            throw new ApplicationException(base._exceptionMessage.ToJson(), exception);
        }
    }
}
