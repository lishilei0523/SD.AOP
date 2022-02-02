using SD.AOP.Core.Interfaces;
using SD.AOP.Core.Models.Entities;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SD.AOP.Core.Mediators
{
    /// <summary>
    /// 日志中介者
    /// </summary>
    public static class LogMediator
    {
        #region # 字段及构造器

        /// <summary>
        /// 日志记录者
        /// </summary>
        private static readonly ILogger _Logger;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static LogMediator()
        {
            Assembly impAssembly = Assembly.Load(AopSection.Setting.LoggerProvider.Assembly);
            Type implType = impAssembly.GetType(AopSection.Setting.LoggerProvider.Type);
            _Logger = (ILogger)Activator.CreateInstance(implType);

        }

        #endregion

        #region # 记录异常日志 —— static Guid Write(ExceptionLog log)
        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="log">异常日志</param>
        /// <returns>日志Id</returns>
        public static Guid Write(ExceptionLog log)
        {
            Guid logId = _Logger.Write(log);

            return logId;
        }
        #endregion

        #region # 记录异常日志 —— static Task<Guid> WriteAsync(ExceptionLog log)
        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="log">异常日志</param>
        /// <returns>日志Id</returns>
        public static async Task<Guid> WriteAsync(ExceptionLog log)
        {
            Task<Guid> logId = Task.Factory.StartNew(() => _Logger.Write(log));

            return await logId;
        }
        #endregion

        #region # 记录运行日志 —— static Guid Write(RunningLog log)
        /// <summary>
        /// 记录运行日志
        /// </summary>
        /// <param name="log">运行日志</param>
        /// <returns>日志Id</returns>
        public static Guid Write(RunningLog log)
        {
            Guid logId = _Logger.Write(log);

            return logId;
        }
        #endregion

        #region # 记录运行日志 —— static Task<Guid> WriteAsync(RunningLog log)
        /// <summary>
        /// 异步记录运行日志
        /// </summary>
        /// <param name="log">运行日志</param>
        /// <returns>日志Id</returns>
        public static async Task<Guid> WriteAsync(RunningLog log)
        {
            Task<Guid> logId = Task.Factory.StartNew(() => _Logger.Write(log));

            return await logId;
        }
        #endregion
    }
}
