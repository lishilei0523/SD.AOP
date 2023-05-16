using SD.AOP.Core.Models.Entities;
using SD.AOP.LogViewer.Models;
using SD.Infrastructure.Constants;
using SD.Toolkits.Mapper;

namespace SD.AOP.LogViewer.Maps
{
    /// <summary>
    /// 日志相关映射
    /// </summary>
    public static class LogMap
    {
        #region # 异常日志映射 —— static ExceptionLogModel ToModel(this ExceptionLog log)
        /// <summary>
        /// 异常日志映射
        /// </summary>
        public static ExceptionLogModel ToModel(this ExceptionLog log)
        {
            ExceptionLogModel logModel = log.Map<ExceptionLog, ExceptionLogModel>(null, null, x => x.OccurredTime);
            logModel.OccurredTime = log.OccurredTime.ToString(CommonConstants.DateTimeFormat);

            return logModel;
        }
        #endregion

        #region # 运行日志映射 —— static RunningLogModel ToModel(this RunningLog log)
        /// <summary>
        /// 运行日志映射
        /// </summary>
        public static RunningLogModel ToModel(this RunningLog log)
        {
            RunningLogModel logModel = log.Map<RunningLog, RunningLogModel>(null, null, x => x.StartTime, x => x.EndTime);
            logModel.StartTime = log.StartTime.ToString(CommonConstants.DateTimeFormat);
            logModel.EndTime = log.EndTime.ToString(CommonConstants.DateTimeFormat);

            return logModel;
        }
        #endregion
    }
}
