using SD.AOP.Core.Models.Entities;
using SD.Common.PoweredByLee;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace SD.AOP.LogViewer.Repositories
{
    /// <summary>
    /// 程序日志仓储接口
    /// </summary>
    public class RunningLogRepository
    {
        #region # 字段及构造器

        /// <summary>
        /// SQL工具
        /// </summary>
        private static readonly SqlHelper _SqlHelper;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static RunningLogRepository()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[Common.DefaultConnectionStringName].ConnectionString;

            //初始化SQL工具
            _SqlHelper = new SqlHelper(connectionString);
        }

        #endregion


        //Public

        #region # 删除运行日志 —— void RemoveRunningLogs(IEnumerable<Guid> ids)
        /// <summary>
        /// 删除运行日志
        /// </summary>
        /// <param name="ids">运行日志Id集</param>
        public void RemoveRunningLogs(IEnumerable<Guid> ids)
        {
            #region # 验证

            Guid[] specIds = ids == null ? new Guid[0] : ids.ToArray();

            if (!specIds.Any())
            {
                return;
            }

            #endregion

            string idsStr = Common.GetIdsString(specIds);

            string sql = "DELETE FROM RunningLogs WHERE Id IN (@Id)";
            SqlParameter arg = new SqlParameter("@Id", idsStr);

            _SqlHelper.ExecuteNonQuery(sql, arg);
        }
        #endregion

        #region # 获取运行日志 —— RunningLog Single(Guid id)
        /// <summary>
        /// 获取运行日志
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>运行日志</returns>
        public RunningLog Single(Guid id)
        {
            string sql = "SELECT * FROM RunningLogs WHERE Id = @Id";

            using (SqlDataReader reader = _SqlHelper.ExecuteReader(sql, new SqlParameter("@Id", id)))
            {
                if (reader.Read())
                {
                    return this.GetEntity(reader);
                }

                throw new NullReferenceException($"Id为\"{id}\"的运行日志不存在！");
            }
        }
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
        public ICollection<RunningLog> GetRunningLogs(Guid? logId, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize, out int pageCount, out int rowCount)
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

            string sql = "SELECT *, ROW_NUMBER() OVER(ORDER BY StartTime DESC) AS RowIndex  FROM dbo.RunningLogs WHERE 0 = 0";

            #region # 条件过滤

            if (logId.HasValue && logId.Value != Guid.Empty)
            {
                sql = $"{sql} AND Id = '{logId}'";
            }
            if (startTime != null)
            {
                sql = $"{sql} AND StartTime >= '{startTime}'";
            }
            if (endTime != null)
            {
                sql = $"{sql} AND StartTime <= '{endTime}'";
            }

            #endregion

            //分页处理
            sql = $"SELECT * FROM ({sql}) AS T WHERE T.RowIndex >= @Start AND T.RowIndex <= @End";

            SqlParameter[] args = {
                new SqlParameter("@Start", start),
                new SqlParameter("@End", end)
            };

            ICollection<RunningLog> runningLogs = new HashSet<RunningLog>();

            using (SqlDataReader reader = _SqlHelper.ExecuteReader(sql, args))
            {
                while (reader.Read())
                {
                    runningLogs.Add(this.GetEntity(reader));
                }
            }
            return runningLogs;
        }
        #endregion

        #region # 获取运行日志记录条数 —— int Count(Guid logId, string startTime...
        /// <summary>
        /// 获取运行日志记录条数
        /// </summary>
        /// <param name="logId">日志Id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>运行日志记录条数</returns>
        public int Count(Guid? logId, DateTime? startTime, DateTime? endTime)
        {
            string sql = "SELECT COUNT(*) FROM dbo.RunningLogs WHERE 0 = 0";

            #region # 条件过滤

            if (logId.HasValue && logId.Value != Guid.Empty)
            {
                sql = $"{sql} AND Id = '{logId}'";
            }
            if (startTime != null)
            {
                sql = $"{sql} AND StartTime >= '{startTime}'";
            }
            if (endTime != null)
            {
                sql = $"{sql} AND StartTime <= '{endTime}'";
            }

            #endregion

            return (int)_SqlHelper.ExecuteScalar(sql);
        }
        #endregion


        //Private

        #region # 根据DataReader获取运行日志 —— RunningLog GetEntity(SqlDataReader reader)
        /// <summary>
        /// 根据DataReader获取实体
        /// </summary>
        /// <param name="reader">DataReader</param>
        /// <returns>运行日志</returns>
        private RunningLog GetEntity(SqlDataReader reader)
        {
            RunningLog runningLog = new RunningLog
            {
                Id = (Guid)Common.ToNetValue(reader, "Id"),
                Namespace = (string)Common.ToNetValue(reader, "Namespace"),
                ClassName = (string)Common.ToNetValue(reader, "ClassName"),
                MethodName = (string)Common.ToNetValue(reader, "MethodName"),
                MethodType = (string)Common.ToNetValue(reader, "MethodType"),
                ArgsJson = (string)Common.ToNetValue(reader, "ArgsJson"),
                ReturnValue = (string)Common.ToNetValue(reader, "ReturnValue"),
                ReturnValueType = (string)Common.ToNetValue(reader, "ReturnValueType"),
                OperatorAccount = (string)Common.ToNetValue(reader, "OperatorAccount"),
                StartTime = (DateTime)Common.ToNetValue(reader, "StartTime"),
                EndTime = (DateTime)Common.ToNetValue(reader, "EndTime"),
                IPAddress = (string)Common.ToNetValue(reader, "IPAddress")
            };
            return runningLog;
        }
        #endregion
    }
}
