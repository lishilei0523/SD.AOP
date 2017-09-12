using PostSharp.Aspects;
using System;

namespace SD.AOP.Core.Aspects.ForAny
{
    /// <summary>
    /// 不抛出异常AOP特性类，只记录日志
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class NotThrowExceptionAspect : ExceptionAspect
    {
        /// <summary>
        /// 异常过滤器
        /// </summary>
        /// <param name="eventArgs">方法元数据</param>
        public override void OnException(MethodExecutionArgs eventArgs)
        {
            base.OnException(eventArgs);

            //方法继续执行
            eventArgs.FlowBehavior = FlowBehavior.Continue;
        }
    }
}