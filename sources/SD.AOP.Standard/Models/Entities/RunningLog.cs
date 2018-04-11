using System;

namespace SD.AOP.Standard.Models.Entities
{
    /// <summary>
    /// 运行日志
    /// </summary>
    [Serializable]
    public class RunningLog : BaseLog
    {
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

        #region 开始时间 —— DateTime StartTime
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        #endregion

        #region 结束时间 —— DateTime EndTime
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        #endregion
    }
}
