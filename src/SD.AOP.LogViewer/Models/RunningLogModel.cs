using System;

namespace SD.AOP.LogViewer.Models
{
    /// <summary>
    /// 运行日志模型
    /// </summary>
    [Serializable]
    public class RunningLogModel
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

        #region 方法返回值 —— string ReturnValue
        /// <summary>
        /// 方法返回值
        /// </summary>
        public string ReturnValue { get; set; }
        #endregion

        #region 方法返回值类型 —— string ReturnValueType
        /// <summary>
        /// 方法返回值
        /// </summary>
        public string ReturnValueType { get; set; }
        #endregion

        #region 操作人账号 —— string OperatorAccount
        /// <summary>
        /// 操作人账号
        /// </summary>
        public string OperatorAccount { get; set; }
        #endregion

        #region 开始时间 —— string StartTime
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        #endregion

        #region 结束时间 —— string EndTime
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
        #endregion
    }
}
