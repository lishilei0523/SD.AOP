using SD.AOP.Core;
using SD.AOP.Core.Interfaces;
using SD.AOP.Core.Models.Entities;
using SD.Toolkits.Sql;
using SD.Toolkits.Sql.SQLite;
using System;
using System.Configuration;
using System.Data;
using System.Text;
#if NET40 || NET45
using System.Data.SQLite;
#endif
#if NET461_OR_GREATER || NETSTANDARD2_0_OR_GREATER
using Microsoft.Data.Sqlite;
#endif

// ReSharper disable once CheckNamespace
namespace SD.AOP
{
    /// <summary>
    /// SQLite日志记录者实现
    /// </summary>
    public class SqliteLogger : ILogger
    {
        #region # 字段及构造器

        /// <summary>
        /// SQL工具
        /// </summary>
        private static readonly SqliteHelper _SqlHelper;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static SqliteLogger()
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
            _SqlHelper = new SqliteHelper(connectionString);

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
            log.Id = Guid.NewGuid();

            //01.构造sql语句
            string sql = "INSERT INTO ExceptionLogs (Id, Namespace, ClassName, MethodName, MethodType, ArgsJson, ExceptionType, ExceptionMessage, ExceptionInfo, InnerException, OccurredTime, IPAddress) VALUES (@Id, @Namespace, @ClassName, @MethodName, @MethodType, @ArgsJson, @ExceptionType, @ExceptionMessage, @ExceptionInfo, @InnerException, @OccurredTime, @IPAddress)";

            //02.初始化参数
            IDbDataParameter[] parameters =
            {
#if NET40 || NET45
                new SQLiteParameter("@Id", log.Id.ToDbValue()),
                new SQLiteParameter("@Namespace", log.Namespace.ToDbValue()),
                new SQLiteParameter("@ClassName", log.ClassName.ToDbValue()),
                new SQLiteParameter("@MethodName", log.MethodName.ToDbValue()),
                new SQLiteParameter("@MethodType", log.MethodType.ToDbValue()),
                new SQLiteParameter("@ArgsJson", log.ArgsJson.ToDbValue()),
                new SQLiteParameter("@ExceptionType", log.ExceptionType.ToDbValue()),
                new SQLiteParameter("@ExceptionMessage", log.ExceptionMessage.ToDbValue()),
                new SQLiteParameter("@ExceptionInfo", log.ExceptionInfo.ToDbValue()),
                new SQLiteParameter("@InnerException", log.InnerException.ToDbValue()),
                new SQLiteParameter("@OccurredTime", log.OccurredTime.ToDbValue()),
                new SQLiteParameter("@IPAddress", log.IPAddress.ToDbValue())
#endif
#if NET461_OR_GREATER || NETSTANDARD2_0_OR_GREATER
                new SqliteParameter("@Id", log.Id.ToDbValue()),
                new SqliteParameter("@Namespace", log.Namespace.ToDbValue()),
                new SqliteParameter("@ClassName", log.ClassName.ToDbValue()),
                new SqliteParameter("@MethodName", log.MethodName.ToDbValue()),
                new SqliteParameter("@MethodType", log.MethodType.ToDbValue()),
                new SqliteParameter("@ArgsJson", log.ArgsJson.ToDbValue()),
                new SqliteParameter("@ExceptionType", log.ExceptionType.ToDbValue()),
                new SqliteParameter("@ExceptionMessage", log.ExceptionMessage.ToDbValue()),
                new SqliteParameter("@ExceptionInfo", log.ExceptionInfo.ToDbValue()),
                new SqliteParameter("@InnerException", log.InnerException.ToDbValue()),
                new SqliteParameter("@OccurredTime", log.OccurredTime.ToDbValue()),
                new SqliteParameter("@IPAddress", log.IPAddress.ToDbValue())
#endif
            };

            //03.执行sql
            _SqlHelper.ExecuteNonQuery(sql, parameters);

            return log.Id;
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
            log.Id = Guid.NewGuid();

            //01.构造SQL语句
            string sql = "INSERT INTO RunningLogs (Id, Namespace, ClassName, MethodName, MethodType, ArgsJson, ReturnValue, ReturnValueType, OperatorAccount, StartTime, EndTime, IPAddress) VALUES (@Id, @Namespace, @ClassName, @MethodName, @MethodType, @ArgsJson, @ReturnValue, @ReturnValueType, @OperatorAccount, @StartTime, @EndTime, @IPAddress)";

            //02.初始化参数
            IDbDataParameter[] parameters =
            {
#if NET40 || NET45
                new SQLiteParameter("@Id", log.Id.ToDbValue()),
                new SQLiteParameter("@Namespace", log.Namespace.ToDbValue()),
                new SQLiteParameter("@ClassName", log.ClassName.ToDbValue()),
                new SQLiteParameter("@MethodName", log.MethodName.ToDbValue()),
                new SQLiteParameter("@MethodType", log.MethodType.ToDbValue()),
                new SQLiteParameter("@ArgsJson", log.ArgsJson.ToDbValue()),
                new SQLiteParameter("@ReturnValue", log.ReturnValue.ToDbValue()),
                new SQLiteParameter("@ReturnValueType", log.ReturnValueType.ToDbValue()),
                new SQLiteParameter("@OperatorAccount", log.OperatorAccount.ToDbValue()),
                new SQLiteParameter("@StartTime", log.StartTime.ToDbValue()),
                new SQLiteParameter("@EndTime", log.EndTime.ToDbValue()),
                new SQLiteParameter("@IPAddress", log.IPAddress.ToDbValue())
#endif
#if NET461_OR_GREATER || NETSTANDARD2_0_OR_GREATER
                new SqliteParameter("@Id", log.Id.ToDbValue()),
                new SqliteParameter("@Namespace", log.Namespace.ToDbValue()),
                new SqliteParameter("@ClassName", log.ClassName.ToDbValue()),
                new SqliteParameter("@MethodName", log.MethodName.ToDbValue()),
                new SqliteParameter("@MethodType", log.MethodType.ToDbValue()),
                new SqliteParameter("@ArgsJson", log.ArgsJson.ToDbValue()),
                new SqliteParameter("@ReturnValue", log.ReturnValue.ToDbValue()),
                new SqliteParameter("@ReturnValueType", log.ReturnValueType.ToDbValue()),
                new SqliteParameter("@OperatorAccount", log.OperatorAccount.ToDbValue()),
                new SqliteParameter("@StartTime", log.StartTime.ToDbValue()),
                new SqliteParameter("@EndTime", log.EndTime.ToDbValue()),
                new SqliteParameter("@IPAddress", log.IPAddress.ToDbValue())
#endif
            };

            //03.执行sql
            _SqlHelper.ExecuteNonQuery(sql, parameters);

            return log.Id;
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
            sqlBuilder.AppendLine("CREATE TABLE IF NOT EXISTS ExceptionLogs ( ");
            sqlBuilder.AppendLine(" Id TEXT PRIMARY KEY NOT NULL, ");
            sqlBuilder.AppendLine(" Namespace TEXT default NULL, ");
            sqlBuilder.AppendLine(" ClassName TEXT default NULL, ");
            sqlBuilder.AppendLine(" MethodName TEXT default NULL, ");
            sqlBuilder.AppendLine(" MethodType TEXT default NULL, ");
            sqlBuilder.AppendLine(" ArgsJson TEXT default NULL, ");
            sqlBuilder.AppendLine(" ExceptionType TEXT default NULL, ");
            sqlBuilder.AppendLine(" ExceptionMessage TEXT default NULL, ");
            sqlBuilder.AppendLine(" ExceptionInfo TEXT default NULL, ");
            sqlBuilder.AppendLine(" InnerException TEXT default NULL, ");
            sqlBuilder.AppendLine(" OccurredTime DATETIME NOT NULL, ");
            sqlBuilder.AppendLine(" IPAddress TEXT default NULL ");
            sqlBuilder.AppendLine(");");


            //初始化程序运行日志
            sqlBuilder.AppendLine("CREATE TABLE IF NOT EXISTS RunningLogs ( ");
            sqlBuilder.AppendLine(" Id TEXT PRIMARY KEY NOT NULL, ");
            sqlBuilder.AppendLine(" Namespace TEXT default NULL, ");
            sqlBuilder.AppendLine(" ClassName TEXT default NULL, ");
            sqlBuilder.AppendLine(" MethodName TEXT default NULL, ");
            sqlBuilder.AppendLine(" MethodType TEXT default NULL, ");
            sqlBuilder.AppendLine(" ArgsJson TEXT default NULL, ");
            sqlBuilder.AppendLine(" ReturnValue TEXT default NULL,");
            sqlBuilder.AppendLine(" ReturnValueType TEXT default NULL,");
            sqlBuilder.AppendLine(" OperatorAccount TEXT default NULL,");
            sqlBuilder.AppendLine(" StartTime DATETIME default NULL,");
            sqlBuilder.AppendLine(" EndTime DATETIME default NULL,");
            sqlBuilder.AppendLine(" IPAddress TEXT default NULL ");
            sqlBuilder.AppendLine(");");

            //执行创建表
            _SqlHelper.ExecuteNonQuery(sqlBuilder.ToString());
        }
        #endregion
    }
}
