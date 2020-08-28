using SD.AOP.Core.Models.Entities;
using SD.AOP.LogViewer.ViewModels.Outputs;
using SD.Common.PoweredByLee;
using SD.Infrastructure.Constants;

namespace SD.AOP.LogViewer.Maps
{
    /// <summary>
    /// 日志相关映射工具类
    /// </summary>
    public static class LogMap
    {
        #region # 异常日志映射 —— static ExceptionLogView ToViewModel(this ExceptionLog log)
        /// <summary>
        /// 异常日志映射
        /// </summary>
        public static ExceptionLogView ToViewModel(this ExceptionLog log)
        {
            ExceptionLogView logView = log.Map<ExceptionLog, ExceptionLogView>(null, null, x => x.OccurredTime);
            logView.OccurredTime = log.OccurredTime.ToString(CommonConstants.TimeFormat);

            return logView;
        }
        #endregion

        #region # 运行日志映射 —— static RunningLogView ToViewModel(this RunningLog log)
        /// <summary>
        /// 运行日志映射
        /// </summary>
        public static RunningLogView ToViewModel(this RunningLog log)
        {
            RunningLogView logView = log.Map<RunningLog, RunningLogView>(null, null, x => x.StartTime, x => x.EndTime);
            logView.StartTime = log.StartTime.ToString(CommonConstants.TimeFormat);
            logView.EndTime = log.EndTime.ToString(CommonConstants.TimeFormat);

            return logView;
        }
        #endregion
    }
}
