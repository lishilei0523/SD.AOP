using PostSharp.Aspects;
using SD.AOP.Standard.Aspects.ForAny;
using System;

namespace SD.AOP.Standard.Tests.StubImplements
{
    /// <summary>
    /// 具体异常AOP特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class StubExceptionLogAspect : ExceptionAspect
    {
        /// <summary>
        /// 异常过滤器
        /// </summary>
        /// <param name="eventArgs">方法元数据</param>
        public override void OnException(MethodExecutionArgs eventArgs)
        {
            base.OnException(eventArgs);

            throw eventArgs.Exception;
        }
    }
}
