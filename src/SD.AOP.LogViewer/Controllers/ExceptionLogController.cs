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
    /// 异常日志控制器
    /// </summary>
    public class ExceptionLogController : Controller
    {
        #region # 字段及构造器

        /// <summary>
        /// 异常日志仓储接口
        /// </summary>
        private readonly IExceptionLogRepository _exceptionLogRep;

        /// <summary>
        /// 构造器
        /// </summary>
        public ExceptionLogController(IExceptionLogRepository exceptionLogRep)
        {
            this._exceptionLogRep = exceptionLogRep;
        }

        #endregion


        //视图部分

        #region # 获取异常日志列表视图 —— ViewResult Index()
        /// <summary>
        /// 获取异常日志列表视图
        /// </summary>
        /// <returns>异常日志列表视图</returns>
        [HttpGet]
        public ViewResult Index()
        {
            return base.View();
        }
        #endregion

        #region # 获取异常日志查看视图 —— ViewResult Detail(Guid id)
        /// <summary>
        /// 获取异常日志查看视图
        /// </summary>
        /// <param name="id">异常日志Id</param>
        /// <returns>异常日志查看视图</returns>
        [HttpGet]
        public ViewResult Detail(Guid id)
        {
            ExceptionLog exceptionLog = this._exceptionLogRep.Single(id);
            ExceptionLogModel exceptionLogModel = exceptionLog.ToModel();

            return base.View(exceptionLogModel);
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
            this._exceptionLogRep.RemoveExceptionLogs(new[] { id });
        }
        #endregion

        #region # 批量删除日志 —— void RemoveLogs(IEnumerable<Guid> logIds)
        /// <summary>
        /// 批量删除日志
        /// </summary>
        /// <param name="logIds">日志Id集</param>
        [HttpPost]
        public void RemoveLogs(IEnumerable<Guid> logIds)
        {
            this._exceptionLogRep.RemoveExceptionLogs(logIds);
        }
        #endregion


        //查询部分

        #region # 获取异常日志列表 —— JsonResult GetExceptionLogs(Guid? logId, DateTime? startTime...
        /// <summary>
        /// 获取异常日志列表
        /// </summary>
        /// <returns>异常日志列表</returns>
        [HttpGet]
        [HttpPost]
        public JsonResult GetExceptionLogs(Guid? logId, DateTime? startTime, DateTime? endTime, int page, int rows)
        {
            ICollection<ExceptionLog> exceptionLogs = this._exceptionLogRep.GetExceptionLogs(logId, startTime, endTime, page, rows, out int _, out int rowCount);
            IEnumerable<ExceptionLogModel> exceptionLogViews = exceptionLogs.Select(x => x.ToModel());
            Grid<ExceptionLogModel> grid = new Grid<ExceptionLogModel>(rowCount, exceptionLogViews);

            return base.Json(grid);
        }
        #endregion
    }
}
