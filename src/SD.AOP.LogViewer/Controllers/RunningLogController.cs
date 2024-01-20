using Microsoft.AspNetCore.Mvc;
using SD.AOP.Core.Models.Entities;
using SD.AOP.LogViewer.Maps;
using SD.AOP.LogViewer.Models;
using SD.AOP.LogViewer.Repository.Interfaces;
using SD.Toolkits.EasyUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SD.AOP.LogViewer.Controllers
{
    /// <summary>
    /// 运行日志控制器
    /// </summary>
    public class RunningLogController : Controller
    {
        #region # 字段及构造器

        /// <summary>
        /// 运行日志仓储接口
        /// </summary>
        private readonly IRunningLogRepository _runningLogRep;

        /// <summary>
        /// 构造器
        /// </summary>
        public RunningLogController(IRunningLogRepository runningLogRep)
        {
            this._runningLogRep = runningLogRep;
        }

        #endregion


        //视图部分

        #region # 获取运行日志列表视图 —— ViewResult Index()
        /// <summary>
        /// 获取运行日志列表视图
        /// </summary>
        /// <returns>运行日志列表视图</returns>
        [HttpGet]
        public ViewResult Index()
        {
            return base.View();
        }
        #endregion

        #region # 获取运行日志查看视图 —— ViewResult Detail(Guid id)
        /// <summary>
        /// 获取运行日志查看视图
        /// </summary>
        /// <param name="id">运行日志Id</param>
        /// <returns>运行日志查看视图</returns>
        [HttpGet]
        public ViewResult Detail(Guid id)
        {
            RunningLog runningLog = this._runningLogRep.Single(id);
            RunningLogModel runningLogModel = runningLog.ToModel();

            return base.View(runningLogModel);
        }
        #endregion


        //命令部分

        #region # 删除日志 —— void RemoveLog(Guid id)
        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="id">日志Id</param>
        [HttpPost]
        public void RemoveLog(Guid id)
        {
            this._runningLogRep.RemoveRunningLogs(new[] { id });
        }
        #endregion

        #region # 批量删除日志 —— void RemoveRunningLogs(IEnumerable<Guid> logIds)
        /// <summary>
        /// 批量删除日志
        /// </summary>
        /// <param name="logIds">日志Id集</param>
        [HttpPost]
        public void RemoveRunningLogs(IEnumerable<Guid> logIds)
        {
            this._runningLogRep.RemoveRunningLogs(logIds);
        }
        #endregion


        //查询部分

        #region # 获取运行日志列表 —— JsonResult GetRunningLogs(Guid? logId, DateTime? startTime...
        /// <summary>
        /// 获取运行日志列表
        /// </summary>
        /// <returns>运行日志列表</returns>
        [HttpGet]
        [HttpPost]
        public JsonResult GetRunningLogs(Guid? logId, DateTime? startTime, DateTime? endTime, int page, int rows)
        {
            ICollection<RunningLog> runningLogs = this._runningLogRep.GetRunningLogs(logId, startTime, endTime, page, rows, out int _, out int rowCount);
            IEnumerable<RunningLogModel> runningLogViews = runningLogs.Select(x => x.ToModel());
            Grid<RunningLogModel> grid = new Grid<RunningLogModel>(rowCount, runningLogViews);

            return base.Json(grid);
        }
        #endregion
    }
}
