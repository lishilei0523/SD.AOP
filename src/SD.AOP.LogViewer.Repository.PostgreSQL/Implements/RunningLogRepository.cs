using Npgsql;
using SD.AOP.Core.Models.Entities;
using SD.AOP.LogViewer.Repository.Interfaces;
using SD.Infrastructure.Constants;
using SD.Toolkits.Sql;
using SD.Toolkits.Sql.PostgreSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SD.AOP.LogViewer.Repository.PostgreSQL.Implements
{
    /// <summary>
    /// 程序日志仓储PostgreSQL实现
    /// </summary>
    public class RunningLogRepository : IRunningLogRepository
    {
        #region # 字段及构造器

        /// <summary>
        /// SQL工具
        /// </summary>
        private static readonly PgSqlHelper _SqlHelper;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static RunningLogRepository()
        {
            //初始化SQL工具
            _SqlHelper = new PgSqlHelper(GlobalSetting.ReadConnectionString);
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

            string guids = specIds.FormatGuids();
            string sql = $"DELETE FROM RunningLogs WHERE \"Id\" IN ({guids})";

            _SqlHelper.ExecuteNonQuery(sql);
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
            string sql = "SELECT * FROM RunningLogs WHERE \"Id\" = @Id";
            using (IDataReader reader = _SqlHelper.ExecuteReader(sql, new NpgsqlParameter("@Id", id)))
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

            string sql = "SELECT * FROM RunningLogs WHERE 0 = 0";

            #region # 条件过滤

            if (logId.HasValue && logId.Value != Guid.Empty)
            {
                sql = $"{sql} AND \"Id\" = '{logId}'";
            }
            if (startTime != null)
            {
                sql = $"{sql} AND \"StartTime\" >= '{startTime}'";
            }
            if (endTime != null)
            {
                sql = $"{sql} AND \"StartTime\" <= '{endTime}'";
            }

            #endregion

            //分页处理
            sql = $"{sql} ORDER BY \"StartTime\" DESC LIMIT {pageSize} OFFSET {(pageIndex - 1) * pageSize}; ";
            ICollection<RunningLog> runningLogs = new HashSet<RunningLog>();
            using (IDataReader reader = _SqlHelper.ExecuteReader(sql))
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
            string sql = "SELECT COUNT(*) FROM RunningLogs WHERE 0 = 0";

            #region # 条件过滤

            if (logId.HasValue && logId.Value != Guid.Empty)
            {
                sql = $"{sql} AND \"Id\" = '{logId}'";
            }
            if (startTime != null)
            {
                sql = $"{sql} AND \"StartTime\" >= '{startTime}'";
            }
            if (endTime != null)
            {
                sql = $"{sql} AND \"StartTime\" <= '{endTime}'";
            }

            #endregion

            object result = _SqlHelper.ExecuteScalar(sql);
            int count = Convert.ToInt32(result);
            return count;
        }
        #endregion


        //Private

        #region # 根据DataReader获取运行日志 —— RunningLog GetEntity(IDataReader reader)
        /// <summary>
        /// 根据DataReader获取实体
        /// </summary>
        /// <param name="reader">DataReader</param>
        /// <returns>运行日志</returns>
        private RunningLog GetEntity(IDataReader reader)
        {
            RunningLog runningLog = new RunningLog
            {
                Id = (Guid)reader.ToClsValue("Id"),
                Namespace = (string)reader.ToClsValue("Namespace"),
                ClassName = (string)reader.ToClsValue("ClassName"),
                MethodName = (string)reader.ToClsValue("MethodName"),
                MethodType = (string)reader.ToClsValue("MethodType"),
                ArgsJson = (string)reader.ToClsValue("ArgsJson"),
                ReturnValue = (string)reader.ToClsValue("ReturnValue"),
                ReturnValueType = (string)reader.ToClsValue("ReturnValueType"),
                OperatorAccount = (string)reader.ToClsValue("OperatorAccount"),
                StartTime = (DateTime)reader.ToClsValue("StartTime"),
                EndTime = (DateTime)reader.ToClsValue("EndTime"),
                IPAddress = (string)reader.ToClsValue("IPAddress")
            };
            return runningLog;
        }
        #endregion
    }
}
