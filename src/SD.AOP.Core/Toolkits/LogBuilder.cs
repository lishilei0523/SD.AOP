using ArxOne.MrAdvice.Advice;
using SD.AOP.Core.Models.Entities;
using SD.AOP.Core.Models.ValueObjects;
using SD.Common;
using SD.Toolkits.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SD.AOP.Core.Toolkits
{
    /// <summary>
    /// 日志建造者
    /// </summary>
    internal static class LogBuilder
    {
        #region # 建造基本信息 —— static void BuildBasicInfo(this BaseLog log, MethodAdviceContext context)
        /// <summary>
        /// 建造基本信息
        /// </summary>
        /// <param name="log">日志</param>
        /// <param name="context">方法元数据</param>
        public static void BuildBasicInfo(this BaseLog log, MethodAdviceContext context)
        {
            log.Namespace = context.TargetMethod.DeclaringType?.Namespace;
            log.ClassName = context.TargetMethod.DeclaringType?.Name;
            log.MethodName = context.TargetMethod.Name;
            log.MethodType = context.TargetMethod.IsStatic ? "静态" : "实例";
            log.IPAddress = CommonExtension.GetLocalIPAddress();
        }
        #endregion

        #region # 建造方法参数信息 —— static void BuildMethodArgsInfo(this BaseLog log, MethodAdviceContext context)
        /// <summary>
        /// 建造方法参数信息
        /// </summary>
        /// <param name="log">日志</param>
        /// <param name="context">方法元数据</param>
        public static void BuildMethodArgsInfo(this BaseLog log, MethodAdviceContext context)
        {
            IList<object> arguments = context.Arguments;
            ParameterInfo[] parameters = context.TargetMethod.GetParameters();

            List<MethodArg> argList = new List<MethodArg>();

            for (int i = 0; arguments != null && i < arguments.Count; i++)
            {
                string argTypeName = $"{parameters[i].ParameterType.Namespace}.{parameters[i].ParameterType.Name}";
                MethodArg arg = new MethodArg(parameters[i].Name, argTypeName, arguments[i].ToJson());

                argList.Add(arg);
            }

            log.ArgsJson = argList.ToJson();
        }
        #endregion

        #region # 建造异常信息 —— static void BuildExceptionInfo(this ExceptionLog log...
        /// <summary>
        /// 建造异常信息
        /// </summary>
        /// <param name="log">异常日志</param>
        /// <param name="exception">异常实例</param>
        public static void BuildExceptionInfo(this ExceptionLog log, Exception exception)
        {
            log.ExceptionType = exception.GetType().Name;
            log.ExceptionMessage = exception.Message;
            log.ExceptionInfo = exception.ToString();
            log.OccurredTime = DateTime.Now;

            StringBuilder exceptionBuilder = new StringBuilder();
            log.InnerException = LogBuilder.BuildInnerException(exceptionBuilder, exception);
        }
        #endregion

        #region # 建造程序运行信息 —— static void BuildRuningInfo(this RunningLog log, MethodAdviceContext context)
        /// <summary>
        /// 建造程序运行信息
        /// </summary>
        /// <param name="log">程序运行日志</param>
        /// <param name="context">方法元数据</param>
        public static void BuildRuningInfo(this RunningLog log, MethodAdviceContext context)
        {
            log.StartTime = DateTime.Now;
            log.OperatorAccount = null;
        }
        #endregion

        #region # 建造返回值信息 —— static void BuildReturnValueInfo(this RunningLog log, MethodAdviceContext context)
        /// <summary>
        /// 建造返回值信息
        /// </summary>
        /// <param name="log">程序运行日志</param>
        /// <param name="context">方法元数据</param>
        public static void BuildReturnValueInfo(this RunningLog log, MethodAdviceContext context)
        {
            log.EndTime = DateTime.Now;

            if (!context.HasReturnValue)
            {
                log.ReturnValue = null;
                log.ReturnValueType = "void";
            }
            else
            {
                log.ReturnValue = context.ReturnValue.ToJson();
                log.ReturnValueType = $"{context.ReturnValue.GetType().Namespace}.{context.ReturnValue.GetType().Name}";
            }
        }
        #endregion

        #region # 建造内部异常 —— static string BuildInnerException(StringBuilder...
        /// <summary>
        /// 建造内部异常
        /// </summary>
        /// <param name="exBuilder">异常建造者</param>
        /// <param name="exception">异常</param>
        /// <returns>内部异常</returns>
        private static string BuildInnerException(StringBuilder exBuilder, Exception exception)
        {
            if (exception.InnerException != null)
            {
                exBuilder.Append(exception.InnerException);
                exBuilder.Append(@"\r\n");
                LogBuilder.BuildInnerException(exBuilder, exception.InnerException);
            }
            return exBuilder.ToString();
        }
        #endregion
    }
}
