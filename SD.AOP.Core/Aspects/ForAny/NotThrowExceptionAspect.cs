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
        public override void OnException(MethodExecutionArgs eventArgs)
        {
            base.OnException(eventArgs);
            eventArgs.FlowBehavior = FlowBehavior.Continue;
        }
    }
}