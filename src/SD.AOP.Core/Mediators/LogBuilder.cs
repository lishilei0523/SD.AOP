﻿using ArxOne.MrAdvice.Advice;
using SD.AOP.Core.Models.Entities;
using SD.AOP.Core.Models.ValueObjects;
using SD.Toolkits.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace SD.AOP.Core.Mediators
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
            log.IPAddress = GetLocalIPAddress();
        }
        #endregion

        #region # 建造参数信息 —— static void BuildParametersInfo(this BaseLog log, MethodAdviceContext context)
        /// <summary>
        /// 建造参数信息
        /// </summary>
        /// <param name="log">日志</param>
        /// <param name="context">方法元数据</param>
        public static void BuildParametersInfo(this BaseLog log, MethodAdviceContext context)
        {
            IList<object> arguments = context.Arguments ?? new List<object>();
            ParameterInfo[] parameters = context.TargetMethod.GetParameters();

            IList<MethodArg> methodArgs = new List<MethodArg>();
            for (int index = 0; index < arguments.Count; index++)
            {
                string argTypeName = parameters[index].ParameterType.FullName;
                MethodArg methodArg = new MethodArg(parameters[index].Name, argTypeName, arguments[index].ToJson());

                methodArgs.Add(methodArg);
            }

            log.ArgsJson = methodArgs.ToJson();
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
            log.InnerException = BuildInnerException(exceptionBuilder, exception);
        }
        #endregion

        #region # 建造运行信息 —— static void BuildRunningInfo(this RunningLog log)
        /// <summary>
        /// 建造运行信息
        /// </summary>
        /// <param name="log">运行日志</param>
        public static void BuildRunningInfo(this RunningLog log)
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
                log.ReturnValueType = context.ReturnValue.GetType().FullName;
            }
        }
        #endregion


        //Private

        #region # 建造内部异常 —— static string BuildInnerException(StringBuilder...
        /// <summary>
        /// 建造内部异常
        /// </summary>
        /// <param name="exceptionBuilder">异常建造者</param>
        /// <param name="exception">异常</param>
        /// <returns>内部异常</returns>
        private static string BuildInnerException(StringBuilder exceptionBuilder, Exception exception)
        {
            if (exception.InnerException != null)
            {
                exceptionBuilder.AppendLine(exception.InnerException.ToString());
                BuildInnerException(exceptionBuilder, exception.InnerException);
            }

            return exceptionBuilder.ToString();
        }
        #endregion

        #region # 获取本机IP地址列表 —— static string GetLocalIPAddress()
        /// <summary>
        /// 获取本机IP地址列表
        /// </summary>
        /// <returns>本机IP地址列表</returns>
        /// <remarks>以“,”分隔</remarks>
        private static string GetLocalIPAddress()
        {
            ICollection<string> ips = new HashSet<string>();

            string hostName = Dns.GetHostName();//本机名   
            ips.Add(hostName);

            IPAddress[] ipAddresses = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6
            foreach (IPAddress ipAddress in ipAddresses)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    ips.Add(ipAddress.ToString());
                }
            }

            string ipsText = ips.Aggregate((x, y) => $"{x},{y}");

            return ipsText;
        }
        #endregion
    }
}
