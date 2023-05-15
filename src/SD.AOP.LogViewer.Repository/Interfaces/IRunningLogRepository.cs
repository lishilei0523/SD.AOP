using SD.AOP.Core.Models.Entities;
using System;
using System.Collections.Generic;

namespace SD.AOP.LogViewer.Repository.Interfaces
{
    /// <summary>
    /// 运行日志仓储接口
    /// </summary>
    public interface IRunningLogRepository
    {
        #region # 删除运行日志 —— void RemoveRunningLogs(IEnumerable<Guid> ids)
        /// <summary>
        /// 删除运行日志
        /// </summary>
        /// <param name="ids">运行日志Id集</param>
        void RemoveRunningLogs(IEnumerable<Guid> ids);
        #endregion

        #region # 获取运行日志 —— RunningLog Single(Guid id)
        /// <summary>
        /// 获取运行日志
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>运行日志</returns>
        RunningLog Single(Guid id);
        #endregion

        #region # 获取运行日志列表 —— ICollection<RunningLog> GetRunningLogs(Guid? logId...
        /// <summary>
        /// 获取运行日志列表
        /// </summary>
        /// <param name="logId">日志Id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="pageCount">符合条件的页数</param>
        /// <param name="rowCount">符合条件的记录条数</param>
        /// <returns>运行日志列表</returns>
        ICollection<RunningLog> GetRunningLogs(Guid? logId, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize, out int pageCount, out int rowCount);
        #endregion

        #region # 获取运行日志记录条数 —— int Count(Guid logId, string startTime...
        /// <summary>
        /// 获取运行日志记录条数
        /// </summary>
        /// <param name="logId">日志Id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>运行日志记录条数</returns>
        int Count(Guid? logId, DateTime? startTime, DateTime? endTime);
        #endregion
    }
}
