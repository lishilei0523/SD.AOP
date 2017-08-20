using SD.AOP.Core.Models.Entities;
using SD.Common.PoweredByLee;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace SD.AOP.LogSite.Repositories
{
    /// <summary>
    /// 异常日志仓储接口
    /// </summary>
    public class ExceptionLogRepository
    {
        #region # 字段及构造器

        /// <summary>
        /// SQL工具
        /// </summary>
        private static readonly SqlHelper _SqlHelper;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static ExceptionLogRepository()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LogConnection"].ConnectionString;

            //初始化SQL工具
            _SqlHelper = new SqlHelper(connectionString);
        }

        #endregion


        //Public

        #region # 删除异常日志 —— void RemoveExceptionLogs(IEnumerable<Guid> ids)
        /// <summary>
        /// 删除异常日志
        /// </summary>
        /// <param name="ids">异常日志Id集</param>
        public void RemoveExceptionLogs(IEnumerable<Guid> ids)
        {
            #region # 验证

            Guid[] specIds = ids == null ? new Guid[0] : ids.ToArray();

            if (!specIds.Any())
            {
                return;
            }

            #endregion

            string idsStr = Common.GetIdsString(specIds);

            string sql = "DELETE FROM ExceptionLogs WHERE Id IN (@Id)";
            SqlParameter arg = new SqlParameter("@Id", idsStr);

            _SqlHelper.ExecuteNonQuery(sql, arg);
        }
        #endregion

        #region # 获取异常日志 —— ExceptionLog Single(Guid id)
        /// <summary>
        /// 获取异常日志
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>异常日志</returns>
        public ExceptionLog Single(Guid id)
        {
            string sql = "SELECT * FROM ExceptionLogs WHERE Id = @Id";

            using (SqlDataReader reader = _SqlHelper.ExecuteReader(sql, new SqlParameter("@Id", id)))
            {
                if (reader.Read())
                {
                    return this.GetEntity(reader);
                }

                throw new NullReferenceException(string.Format("Id为\"{0}\"的日志不存在！", id));
            }
        }
        #endregion

        #region # 获取异常日志列表 —— ICollection<ExceptionLog> GetExceptionLogs(Guid? logId...
        /// <summary>
        /// 获取异常日志列表
        /// </summary>
        /// <param name="logId">日志Id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="pageCount">符合条件的页数</param>
        /// <param name="rowCount">符合条件的记录条数</param>
        /// <returns>异常日志列表</returns>
        public ICollection<ExceptionLog> GetExceptionLogs(Guid? logId, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize, out int pageCount, out int rowCount)
        {
            //计算总记录条数与总页数
            rowCount = this.Count(logId, startTime, endTime);
            pageCount = (int)Math.Ceiling((rowCount * 1.0 / pageSize));

            //页索引处理
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageIndex = pageIndex >= pageCount ? pageCount : pageIndex;

            //计算起始行与终止行
            int start = (pageIndex - 1) * pageSize + 1;
            int end = pageIndex * pageSize;

            string sql = "SELECT *, ROW_NUMBER() OVER(ORDER BY OccurredTime DESC) AS RowIndex  FROM dbo.ExceptionLogs WHERE 0 = 0";

            #region # 条件过滤

            if (logId.HasValue && logId.Value != Guid.Empty)
            {
                sql = string.Format("{0} AND Id = '{1}'", sql, logId);
            }
            if (startTime != null)
            {
                sql = string.Format("{0} AND OccurredTime >= '{1}'", sql, startTime);
            }
            if (endTime != null)
            {
                sql = string.Format("{0} AND OccurredTime <= '{1}'", sql, endTime);
            }

            #endregion

            //分页处理
            sql = string.Format("SELECT * FROM ({0}) AS T WHERE T.RowIndex >= @Start AND T.RowIndex <= @End", sql);

            SqlParameter[] args = {
                new SqlParameter("@Start", start),
                new SqlParameter("@End", end)
            };

            ICollection<ExceptionLog> exceptionLogs = new HashSet<ExceptionLog>();

            using (SqlDataReader reader = _SqlHelper.ExecuteReader(sql, args))
            {
                while (reader.Read())
                {
                    exceptionLogs.Add(this.GetEntity(reader));
                }
            }
            return exceptionLogs;
        }
        #endregion

        #region # 获取异常日志记录条数 —— int Count(Guid logId, string startTime...
        /// <summary>
        /// 获取异常日志记录条数
        /// </summary>
        /// <param name="logId">日志Id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>异常日志记录条数</returns>
        public int Count(Guid? logId, DateTime? startTime, DateTime? endTime)
        {
            string sql = "SELECT COUNT(*) FROM dbo.ExceptionLogs WHERE 0 = 0";

            #region # 条件过滤

            if (logId.HasValue && logId.Value != Guid.Empty)
            {
                sql = string.Format("{0} AND Id = '{1}'", sql, logId);
            }
            if (startTime != null)
            {
                sql = string.Format("{0} AND OccurredTime >= '{1}'", sql, startTime);
            }
            if (endTime != null)
            {
                sql = string.Format("{0} AND OccurredTime <= '{1}'", sql, endTime);
            }

            #endregion

            return (int)_SqlHelper.ExecuteScalar(sql);
        }
        #endregion


        //Private

        #region # 根据DataReader获取异常日志 —— ExceptionLog GetEntity(SqlDataReader reader)
        /// <summary>
        /// 根据DataReader获取实体
        /// </summary>
        /// <param name="reader">DataReader</param>
        /// <returns>异常日志</returns>
        private ExceptionLog GetEntity(SqlDataReader reader)
        {
            ExceptionLog exceptionLog = new ExceptionLog
            {
                Id = (Guid)Common.ToNetValue(reader, "Id"),
                Namespace = (string)Common.ToNetValue(reader, "Namespace"),
                ClassName = (string)Common.ToNetValue(reader, "ClassName"),
                MethodName = (string)Common.ToNetValue(reader, "MethodName"),
                ArgsJson = (string)Common.ToNetValue(reader, "ArgsJson"),
                ExceptionType = (string)Common.ToNetValue(reader, "ExceptionType"),
                ExceptionMessage = (string)Common.ToNetValue(reader, "ExceptionMessage"),
                ExceptionInfo = (string)Common.ToNetValue(reader, "ExceptionInfo"),
                InnerException = (string)Common.ToNetValue(reader, "InnerException"),
                OccurredTime = (DateTime)Common.ToNetValue(reader, "OccurredTime"),
                IPAddress = (string)Common.ToNetValue(reader, "IPAddress"),
                OccurredTimeString = Convert.ToDateTime(Common.ToNetValue(reader, "OccurredTime")).ToString("yyyy-MM-dd HH:mm:ss")
            };
            return exceptionLog;
        }
        #endregion
    }
}
