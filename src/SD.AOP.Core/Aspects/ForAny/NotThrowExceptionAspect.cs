using System;

namespace SD.AOP.Core.Aspects.ForAny
{
    /// <summary>
    /// 不抛出异常AOP特性
    /// </summary>
    /// <remarks>只记录日志</remarks>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class NotThrowExceptionAspect : ExceptionAspect
    {

    }
}