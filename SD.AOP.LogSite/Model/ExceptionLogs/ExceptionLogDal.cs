﻿//------------------------------------------------------------------------------
// <Auto-Generated>
//   Powered By S. L. Lee, ContactQQ：121283087
// </Auto-Generated>
//------------------------------------------------------------------------------

using SD.AOP.Core.Models.Entities;
using SD.AOP.Core.Toolkits;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;

namespace SD.AOP.LogSite.Model.ExceptionLogs
{
    /// <summary>
    /// ExceptionLog数据访问层类
    /// </summary>
    public class ExceptionLogDal
    {
        #region 00.单例构造器

        /// <summary>
        /// SQL工具
        /// </summary>
        private static readonly SqlHelper _SqlHelper;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static ExceptionLogDal()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["LogConnection"].ConnectionString;

            //初始化SQL工具
            _SqlHelper = new SqlHelper(connectionString);
        }

        /// <summary>
        /// 创建对象静态方法
        /// </summary>
        /// <returns>ExceptionLogDal实例</returns>
        public static ExceptionLogDal CreateInstance()
        {
            ExceptionLogDal exceptionLogDal = CallContext.GetData(typeof(ExceptionLogDal).Name) as ExceptionLogDal;
            if (exceptionLogDal == null)
            {
                exceptionLogDal = new ExceptionLogDal();
                CallContext.SetData(typeof(ExceptionLogDal).Name, exceptionLogDal);
            }
            return exceptionLogDal;
        }
        #endregion

        #region 01.删除方法（物理删除）
        /// <summary>
        /// 删除一个实体对象
        /// </summary>
        /// <param name="id">要删除的实体对象Id</param>
        /// <returns>受影响的行数</returns>
        public int PhysicalDelete(Guid id)
        {
            string sql = "DELETE FROM ExceptionLogs WHERE Id = @Id";
            SqlParameter arg = new SqlParameter("@Id", id);
            return _SqlHelper.ExecuteNonQuery(sql, arg);
        }
        #endregion

        #region 02.根据日志Id、开始时间、结束时间查询日志记录条数
        /// <summary>
        /// 根据日志Id、开始时间、结束时间查询日志记录条数
        /// </summary>
        /// <param name="logId">日志Id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>符合条件的日志记录条数</returns>
        public int GetCount(Guid logId, string startTime, string endTime)
        {
            string sql = "SELECT COUNT(*) FROM dbo.ExceptionLogs WHERE 0 = 0";

            #region # 非空校验

            if (logId != Guid.Empty)
            {
                sql = string.Format("{0} AND Id = '{1}'", sql, logId);
            }

            if (!string.IsNullOrWhiteSpace(startTime))
            {
                sql = string.Format("{0} AND OccurredTime >= '{1}'", sql, startTime);
            }

            if (!string.IsNullOrWhiteSpace(endTime))
            {
                sql = string.Format("{0} AND OccurredTime <= '{1}'", sql, endTime);
            }

            #endregion

            return (int)_SqlHelper.ExecuteScalar(sql);
        }
        #endregion

        #region 03.根据日志Id、开始时间、结束时间查询日志列表
        /// <summary>
        /// 根据日志Id、开始时间、结束时间查询日志列表
        /// </summary>
        /// <param name="start">起始行</param>
        /// <param name="end">结束行</param>
        /// <param name="logId">日志Id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>日志列表</returns>
        public List<ExceptionLog> GetModelList(int start, int end, Guid logId, string startTime, string endTime)
        {
            List<ExceptionLog> list = new List<ExceptionLog>();
            string sql = "SELECT *, ROW_NUMBER() OVER(ORDER BY OccurredTime DESC) AS RowIndex  FROM dbo.ExceptionLogs WHERE 0 = 0";

            #region # 非空校验

            if (logId != Guid.Empty)
            {
                sql = string.Format("{0} AND Id = '{1}'", sql, logId);
            }

            if (!string.IsNullOrWhiteSpace(startTime))
            {
                sql = string.Format("{0} AND OccurredTime >= '{1}'", sql, startTime);
            }

            if (!string.IsNullOrWhiteSpace(endTime))
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

            using (SqlDataReader reader = _SqlHelper.ExecuteReader(sql, args))
            {
                while (reader.Read())
                {
                    list.Add(this.ToModel(reader));
                }
            }
            return list;
        }
        #endregion

        #region 04.查看日志明细
        /// <summary>
        /// 查看日志明细
        /// </summary>
        /// <param name="id">标识Id</param>
        /// <returns>日志明细</returns>
        public ExceptionLog GetModel(Guid id)
        {
            string sql = "SELECT * FROM ExceptionLogs WHERE Id = @Id";
            using (SqlDataReader reader = _SqlHelper.ExecuteReader(sql, new SqlParameter("@Id", id)))
            {
                if (reader.Read())
                {
                    return this.ToModel(reader);
                }
                return null;
            }
        }
        #endregion

        #region 05.根据SqlDataReader对象返回实体对象方法
        /// <summary>
        /// 根据SqlDataReader对象返回实体对象方法
        /// </summary>
        /// <param name="reader">SqlDataReader对象</param>
        /// <returns>实体</returns>
        private ExceptionLog ToModel(SqlDataReader reader)
        {
            ExceptionLog exceptionLog = new ExceptionLog
            {
                Id = (Guid)this.ToModelValue(reader, "Id"),
                Namespace = (string)this.ToModelValue(reader, "Namespace"),
                ClassName = (string)this.ToModelValue(reader, "ClassName"),
                MethodName = (string)this.ToModelValue(reader, "MethodName"),
                ArgsJson = (string)this.ToModelValue(reader, "ArgsJson"),
                ExceptionType = (string)this.ToModelValue(reader, "ExceptionType"),
                ExceptionMessage = (string)this.ToModelValue(reader, "ExceptionMessage"),
                ExceptionInfo = (string)this.ToModelValue(reader, "ExceptionInfo"),
                InnerException = (string)this.ToModelValue(reader, "InnerException"),
                OccurredTime = (DateTime)this.ToModelValue(reader, "OccurredTime"),
                IPAddress = (string)this.ToModelValue(reader, "IPAddress"),
                OccurredTimeString = Convert.ToDateTime(this.ToModelValue(reader, "OccurredTime")).ToString("yyyy-MM-dd HH:mm:ss")
            };
            return exceptionLog;
        }
        #endregion

        #region 06.数据库值转C#值空值处理
        /// <summary>
        /// 数据库值转C#值空值处理
        /// </summary>
        /// <param name="reader">IDataReader对象</param>
        /// <param name="columnName">列名</param>
        /// <returns>C#值</returns>
        private object ToModelValue(IDataReader reader, string columnName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(columnName)))
            {
                return null;
            }
            else
            {
                return reader[columnName];
            }
        }
        #endregion
    }
}
