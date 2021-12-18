using SD.AOP.Core.Models.Entities;
using System;

namespace SD.AOP.Core.Interfaces
{
    /// <summary>
    /// 日志记录者接口
    /// </summary>
    public interface ILogger
    {
        #region # 记录异常日志 —— Guid Write(ExceptionLog log)
        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="log">异常日志</param>
        /// <returns>日志Id</returns>
        Guid Write(ExceptionLog log);
        #endregion

        #region # 记录运行日志 —— Guid Write(RunningLog log)
        /// <summary>
        /// 记录运行日志
        /// </summary>
        /// <param name="log">运行日志</param>
        /// <returns>日志Id</returns>
        Guid Write(RunningLog log);
        #endregion
    }
}
