using Npgsql;
using SD.AOP.Core;
using SD.AOP.Core.Interfaces;
using SD.AOP.Core.Models.Entities;
using SD.Toolkits.Sql;
using SD.Toolkits.Sql.PostgreSQL;
using System;
using System.Configuration;
using System.Data;
using System.Text;

// ReSharper disable once CheckNamespace
namespace SD.AOP
{
    /// <summary>
    /// PostgreSQL日志记录者实现
    /// </summary>
    public class PgSqlLogger : ILoggger
    {
        #region # 常量、字段及构造器

        /// <summary>
        /// SQL工具
        /// </summary>
        private static readonly PgSqlHelper _SqlHelper;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static PgSqlLogger()
        {
            string connectionString = null;

            #region # 验证

            if (!string.IsNullOrWhiteSpace(AopSection.Setting.ConnectionString.Name))
            {
                connectionString = ConfigurationManager.ConnectionStrings[AopSection.Setting.ConnectionString.Name]?.ConnectionString;
            }
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = AopSection.Setting.ConnectionString.Value;
            }
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new NullReferenceException("连接字符串未配置！");
            }

            #endregion

            //初始化SQL工具
            _SqlHelper = new PgSqlHelper(connectionString);

            //初始化日志数据表
            InitTable();
        }

        #endregion

        #region # 记录异常日志 —— Guid Write(ExceptionLog log)
        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="log">异常日志</param>
        /// <returns>日志Id</returns>
        public Guid Write(ExceptionLog log)
        {
            //01.构造sql语句
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(
                "INSERT INTO ExceptionLogs (\"Id\", \"Namespace\", \"ClassName\", \"MethodName\", \"MethodType\", \"ArgsJson\", \"ExceptionType\", \"ExceptionMessage\", \"ExceptionInfo\", \"InnerException\", \"OccurredTime\", \"IPAddress\") VALUES (uuid_generate_v4(), @Namespace, @ClassName, @MethodName, @MethodType, @ArgsJson, @ExceptionType, @ExceptionMessage, @ExceptionInfo, @InnerException, @OccurredTime, @IPAddress) ");
            sqlBuilder.Append("RETURNING \"Id\"; ");

            //02.初始化参数
            IDbDataParameter[] parameters = {
                    new NpgsqlParameter("@Namespace",log.Namespace.ToDbValue()),
                    new NpgsqlParameter("@ClassName", log.ClassName.ToDbValue()),
                    new NpgsqlParameter("@MethodName", log.MethodName.ToDbValue()),
                    new NpgsqlParameter("@MethodType", log.MethodType.ToDbValue()),
                    new NpgsqlParameter("@ArgsJson", log.ArgsJson.ToDbValue()),
                    new NpgsqlParameter("@ExceptionType", log.ExceptionType.ToDbValue()),
                    new NpgsqlParameter("@ExceptionMessage", log.ExceptionMessage.ToDbValue()),
                    new NpgsqlParameter("@ExceptionInfo", log.ExceptionInfo.ToDbValue()),
                    new NpgsqlParameter("@InnerException", log.InnerException.ToDbValue()),
                    new NpgsqlParameter("@OccurredTime", log.OccurredTime.ToDbValue()),
                    new NpgsqlParameter("@IPAddress", log.IPAddress.ToDbValue())
                };

            //03.执行sql
            object result = _SqlHelper.ExecuteScalar(sqlBuilder.ToString(), parameters);
            Guid newId = Guid.Parse(result.ToString());
            return newId;
        }
        #endregion

        #region # 记录运行日志 —— Guid Write(RunningLog log)
        /// <summary>
        /// 记录运行日志
        /// </summary>
        /// <param name="log">运行日志</param>
        /// <returns>日志Id</returns>
        public Guid Write(RunningLog log)
        {
            //01.构造SQL语句
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT INTO RunningLogs (\"Id\", \"Namespace\", \"ClassName\", \"MethodName\", \"MethodType\", \"ArgsJson\", \"ReturnValue\", \"ReturnValueType\", \"OperatorAccount\", \"StartTime\", \"EndTime\", \"IPAddress\") VALUES (uuid_generate_v4(), @Namespace, @ClassName, @MethodName, @MethodType, @ArgsJson, @ReturnValue, @ReturnValueType, @OperatorAccount, @StartTime, @EndTime, @IPAddress) ");
            sqlBuilder.Append("RETURNING \"Id\"; ");

            //02.初始化参数
            IDbDataParameter[] parameters = {
                    new NpgsqlParameter("@Namespace", log.Namespace.ToDbValue()),
                    new NpgsqlParameter("@ClassName", log.ClassName.ToDbValue()),
                    new NpgsqlParameter("@MethodName", log.MethodName.ToDbValue()),
                    new NpgsqlParameter("@MethodType", log.MethodType.ToDbValue()),
                    new NpgsqlParameter("@ArgsJson", log.ArgsJson.ToDbValue()),
                    new NpgsqlParameter("@ReturnValue", log.ReturnValue.ToDbValue()),
                    new NpgsqlParameter("@ReturnValueType", log.ReturnValueType.ToDbValue()),
                    new NpgsqlParameter("@OperatorAccount", log.OperatorAccount.ToDbValue()),
                    new NpgsqlParameter("@StartTime", log.StartTime.ToDbValue()),
                    new NpgsqlParameter("@EndTime", log.EndTime.ToDbValue()),
                    new NpgsqlParameter("@IPAddress", log.IPAddress.ToDbValue())
                };

            //03.执行sql
            object result = _SqlHelper.ExecuteScalar(sqlBuilder.ToString(), parameters);
            Guid newId = Guid.Parse(result.ToString());
            return newId;
        }
        #endregion

        #region # 初始化日志数据表 —— static void InitTable()
        /// <summary>
        /// 初始化日志数据表
        /// </summary>
        private static void InitTable()
        {
            //构造sql语句
            StringBuilder sqlBuilder = new StringBuilder();

            //初始化异常日志
            sqlBuilder.Append("CREATE TABLE IF NOT EXISTS ExceptionLogs (\"Id\" uuid NOT NULL PRIMARY KEY, \"Namespace\" text NULL, \"ClassName\" text NULL, \"MethodName\" text NULL, \"MethodType\" text NULL, \"ArgsJson\" text NULL, \"ExceptionType\" text NULL, \"ExceptionMessage\" text NULL, \"ExceptionInfo\" text NULL, \"InnerException\" text NULL, \"OccurredTime\" timestamp NULL, \"IPAddress\" text NULL); ");

            //初始化程序运行日志
            sqlBuilder.Append("CREATE TABLE IF NOT EXISTS RunningLogs (\"Id\" uuid NOT NULL PRIMARY KEY, \"Namespace\" text NULL, \"ClassName\" text NULL, \"MethodName\" text NULL, \"MethodType\" text NULL, \"ArgsJson\" text NULL, \"ReturnValue\" text NULL, \"ReturnValueType\" text NULL, \"OperatorAccount\" text NULL, \"StartTime\" timestamp NULL, \"EndTime\" timestamp NULL, \"IPAddress\" text NULL); ");

            //初始化UUID扩展
            sqlBuilder.Append("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"; ");

            //执行创建表
            _SqlHelper.ExecuteNonQuery(sqlBuilder.ToString());
        }
        #endregion
    }
}
