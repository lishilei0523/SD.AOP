using System;

namespace SD.AOP.LogViewer.Models
{
    /// <summary>
    /// 异常日志模型
    /// </summary>
    [Serializable]
    public class ExceptionLogModel
    {
        #region 标识Id —— Guid Id
        /// <summary>
        /// 标识Id
        /// </summary>
        public Guid Id { get; set; }
        #endregion

        #region 命名空间 —— string Namespace
        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace { get; set; }
        #endregion

        #region 类名 —— string ClassName
        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }
        #endregion

        #region 方法名 —— string MethodName
        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }
        #endregion

        #region 方法类型 —— string MethodType
        /// <summary>
        /// 方法类型
        /// </summary>
        public string MethodType { get; set; }
        #endregion

        #region 方法参数Json —— string ArgsJson
        /// <summary>
        /// 方法参数Json
        /// </summary>
        public string ArgsJson { get; set; }
        #endregion

        #region IP地址 —— string IPAddress
        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; set; }
        #endregion

        #region 异常类型 —— string ExceptionType
        /// <summary>
        /// 异常类型
        /// </summary>
        public string ExceptionType { get; set; }
        #endregion

        #region 异常消息 —— string ExceptionMessage
        /// <summary>
        /// 异常消息
        /// </summary>
        public string ExceptionMessage { get; set; }
        #endregion

        #region 异常详细信息 —— string ExceptionInfo
        /// <summary>
        /// 异常详细信息
        /// </summary>
        public string ExceptionInfo { get; set; }
        #endregion

        #region 内部异常 —— string InnerException
        /// <summary>
        /// 内部异常
        /// </summary>
        public string InnerException { get; set; }
        #endregion

        #region 异常发生时间 —— string OccurredTime
        /// <summary>
        /// 异常发生时间
        /// </summary>
        public string OccurredTime { get; set; }
        #endregion
    }
}
