using SD.AOP.Interfaces;
using SD.AOP.Models.Entities;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SD.AOP.Mediators
{
    /// <summary>
    /// 日志中介者
    /// </summary>
    public static class LogMediator
    {
        #region # 字段及构造器

        /// <summary>
        /// 日志记录者实现类型
        /// </summary>
        private static readonly Type _LoggerImplType;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static LogMediator()
        {
            Assembly impAssembly = Assembly.Load(LoggerProviderConfiguration.Setting.Assembly);
            LogMediator._LoggerImplType = impAssembly.GetType(LoggerProviderConfiguration.Setting.Type);

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
            ILoggger loggger = (ILoggger)Activator.CreateInstance(LogMediator._LoggerImplType);
            Guid logId = loggger.Write(log);

            return logId;
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
            ILoggger loggger = (ILoggger)Activator.CreateInstance(LogMediator._LoggerImplType);
            Guid logId = loggger.Write(log);

            return logId;
        }
        #endregion

        #region # 异步记录异常日志 —— static Task<Guid> WriteAsync(ExceptionLog log)
        /// <summary>
        /// 异步记录异常日志
        /// </summary>
        /// <param name="log">异常日志</param>
        /// <returns>日志Id</returns>
        public static async Task<Guid> WriteAsync(ExceptionLog log)
        {
            ILoggger loggger = (ILoggger)Activator.CreateInstance(LogMediator._LoggerImplType);
            Task<Guid> logId = Task.Run(() => loggger.Write(log));

            return await logId;
        }
        #endregion

        #region # 异步记录运行日志 —— static async Task<Guid> WriteAsync(RunningLog log)
        /// <summary>
        /// 异步记录运行日志
        /// </summary>
        /// <param name="log">运行日志</param>
        /// <returns>日志Id</returns>
        public static async Task<Guid> WriteAsync(RunningLog log)
        {
            ILoggger loggger = (ILoggger)Activator.CreateInstance(LogMediator._LoggerImplType);
            Task<Guid> logId = Task.Run(() => loggger.Write(log));

            return await logId;
        }
        #endregion
    }
}
