using SD.AOP.Core.Models.Entities;
using SD.AOP.LogSite.Maps;
using SD.AOP.LogSite.Repositories;
using SD.AOP.LogSite.ViewModels.Outputs;
using SD.FormatModel.EasyUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SD.AOP.LogSite.Controllers
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
        private readonly ExceptionLogRepository _exceptionLogRep;

        /// <summary>
        /// 构造器
        /// </summary>
        public ExceptionLogController()
        {
            this._exceptionLogRep = new ExceptionLogRepository();
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
            return this.View();
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
            ExceptionLogView exceptionLogView = exceptionLog.ToViewModel();

            return this.View(exceptionLogView);
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
        public JsonResult GetExceptionLogs(Guid? logId, DateTime? startTime, DateTime? endTime, int page, int rows)
        {
            int rowCount, pageCount;

            ICollection<ExceptionLog> exceptionLogs = this._exceptionLogRep.GetExceptionLogs(logId, startTime, endTime, page, rows, out pageCount, out rowCount);
            IEnumerable<ExceptionLogView> exceptionLogViews = exceptionLogs.Select(x => x.ToViewModel());

            Grid<ExceptionLogView> grid = new Grid<ExceptionLogView>(rowCount, exceptionLogViews);

            return base.Json(grid, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
