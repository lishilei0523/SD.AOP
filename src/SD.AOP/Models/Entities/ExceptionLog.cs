﻿using System;

namespace SD.AOP.Models.Entities
{
    /// <summary>
    /// 异常日志
    /// </summary>
    [Serializable]
    public class ExceptionLog : BaseLog
    {
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

        #region 异常发生时间 —— DateTime OccurredTime
        /// <summary>
        /// 异常发生时间
        /// </summary>
        public DateTime OccurredTime { get; set; }
        #endregion
    }
}
