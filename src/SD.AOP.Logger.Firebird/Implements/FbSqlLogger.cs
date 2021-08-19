using FirebirdSql.Data.FirebirdClient;
using SD.AOP.Core;
using SD.AOP.Core.Interfaces;
using SD.AOP.Core.Models.Entities;
using SD.Toolkits.Sql;
using SD.Toolkits.Sql.Firebird;
using System;
using System.Configuration;
using System.Data;
using System.Text;

// ReSharper disable once CheckNamespace
namespace SD.AOP
{
    /// <summary>
    /// Firebird日志记录者实现
    /// </summary>
    public class FbSqlLogger : ILoggger
    {
        #region # 常量、字段及构造器

        /// <summary>
        /// SQL工具
        /// </summary>
        private static readonly FbSqlHelper _SqlHelper;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static FbSqlLogger()
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
            _SqlHelper = new FbSqlHelper(connectionString);
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
            //初始化日志数据表
            InitTable();

            //生成GUID
            string generateIdSql = "SELECT GEN_UUID() FROM rdb$database;";
            object result = _SqlHelper.ExecuteScalar(generateIdSql);
            Guid serialSeedId = Guid.Parse(result.ToString());

            //01.构造sql语句
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(
                "INSERT INTO \"ExceptionLogs\" (\"Id\", \"Namespace\", \"ClassName\", \"MethodName\", \"MethodType\", \"ArgsJson\", \"ExceptionType\", \"ExceptionMessage\", \"ExceptionInfo\", \"InnerException\", \"OccurredTime\", \"IPAddress\") VALUES (@Id, @Namespace, @ClassName, @MethodName, @MethodType, @ArgsJson, @ExceptionType, @ExceptionMessage, @ExceptionInfo, @InnerException, @OccurredTime, @IPAddress); ");

            //02.初始化参数
            IDbDataParameter[] parameters = {
                    new FbParameter("@Id", serialSeedId.ToDbValue()),
                    new FbParameter("@Namespace", log.Namespace.ToDbValue()),
                    new FbParameter("@ClassName", log.ClassName.ToDbValue()),
                    new FbParameter("@MethodName", log.MethodName.ToDbValue()),
                    new FbParameter("@MethodType", log.MethodType.ToDbValue()),
                    new FbParameter("@ArgsJson", log.ArgsJson.ToDbValue()),
                    new FbParameter("@ExceptionType", log.ExceptionType.ToDbValue()),
                    new FbParameter("@ExceptionMessage", log.ExceptionMessage.ToDbValue()),
                    new FbParameter("@ExceptionInfo", log.ExceptionInfo.ToDbValue()),
                    new FbParameter("@InnerException", log.InnerException.ToDbValue()),
                    new FbParameter("@OccurredTime", log.OccurredTime.ToDbValue()),
                    new FbParameter("@IPAddress", log.IPAddress.ToDbValue())
                };

            //03.执行sql
            _SqlHelper.ExecuteNonQuery(sqlBuilder.ToString(), parameters);
            return serialSeedId;
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
            //初始化日志数据表
            InitTable();

            //生成GUID
            string generateIdSql = "SELECT GEN_UUID() FROM rdb$database;";
            object result = _SqlHelper.ExecuteScalar(generateIdSql);
            Guid serialSeedId = Guid.Parse(result.ToString());

            //01.构造SQL语句
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("INSERT INTO \"RunningLogs\" (\"Id\", \"Namespace\", \"ClassName\", \"MethodName\", \"MethodType\", \"ArgsJson\", \"ReturnValue\", \"ReturnValueType\", \"OperatorAccount\", \"StartTime\", \"EndTime\", \"IPAddress\") VALUES (@Id, @Namespace, @ClassName, @MethodName, @MethodType, @ArgsJson, @ReturnValue, @ReturnValueType, @OperatorAccount, @StartTime, @EndTime, @IPAddress); ");

            //02.初始化参数
            IDbDataParameter[] parameters = {
                    new FbParameter("@Id", serialSeedId.ToDbValue()),
                    new FbParameter("@Namespace", log.Namespace.ToDbValue()),
                    new FbParameter("@ClassName", log.ClassName.ToDbValue()),
                    new FbParameter("@MethodName", log.MethodName.ToDbValue()),
                    new FbParameter("@MethodType", log.MethodType.ToDbValue()),
                    new FbParameter("@ArgsJson", log.ArgsJson.ToDbValue()),
                    new FbParameter("@ReturnValue", log.ReturnValue.ToDbValue()),
                    new FbParameter("@ReturnValueType", log.ReturnValueType.ToDbValue()),
                    new FbParameter("@OperatorAccount", log.OperatorAccount.ToDbValue()),
                    new FbParameter("@StartTime", log.StartTime.ToDbValue()),
                    new FbParameter("@EndTime", log.EndTime.ToDbValue()),
                    new FbParameter("@IPAddress", log.IPAddress.ToDbValue())
                };

            //03.执行sql
            _SqlHelper.ExecuteNonQuery(sqlBuilder.ToString(), parameters);
            return serialSeedId;
        }
        #endregion

        #region # 初始化日志数据表 —— static void InitTable()
        /// <summary>
        /// 初始化日志数据表
        /// </summary>
        private static void InitTable()
        {
            //初始化异常日志表
            string exceptionLogsPredicate = "SELECT 'true' FROM rdb$relations WHERE rdb$relation_name = 'ExceptionLogs';";
            object exceptionLogsExists = _SqlHelper.ExecuteScalar(exceptionLogsPredicate);
            if (exceptionLogsExists?.ToString() != "true")
            {
                string sql = "CREATE TABLE \"ExceptionLogs\" (\"Id\" CHAR(16) CHARACTER SET OCTETS NOT NULL, \"Namespace\" BLOB SUB_TYPE TEXT, \"ClassName\" BLOB SUB_TYPE TEXT, \"MethodName\" BLOB SUB_TYPE TEXT, \"MethodType\" BLOB SUB_TYPE TEXT, \"ArgsJson\" BLOB SUB_TYPE TEXT, \"ExceptionType\" BLOB SUB_TYPE TEXT, \"ExceptionMessage\" BLOB SUB_TYPE TEXT, \"ExceptionInfo\" BLOB SUB_TYPE TEXT, \"InnerException\" BLOB SUB_TYPE TEXT, \"OccurredTime\" TIMESTAMP, \"IPAddress\" BLOB SUB_TYPE TEXT, CONSTRAINT \"PK_ExceptionLogs\" PRIMARY KEY (\"Id\")); ";

                //执行创建表
                _SqlHelper.ExecuteNonQuery(sql);
            }

            //初始化程序运行日志表
            string runningLogsPredicate = "SELECT 'true' FROM rdb$relations WHERE rdb$relation_name = 'RunningLogs';";
            object runningLogsExists = _SqlHelper.ExecuteScalar(runningLogsPredicate);
            if (runningLogsExists?.ToString() != "true")
            {
                string sql = "CREATE TABLE \"RunningLogs\" (\"Id\" CHAR(16) CHARACTER SET OCTETS NOT NULL, \"Namespace\" BLOB SUB_TYPE TEXT, \"ClassName\" BLOB SUB_TYPE TEXT, \"MethodName\" BLOB SUB_TYPE TEXT, \"MethodType\" BLOB SUB_TYPE TEXT, \"ArgsJson\" BLOB SUB_TYPE TEXT, \"ReturnValue\" BLOB SUB_TYPE TEXT, \"ReturnValueType\" BLOB SUB_TYPE TEXT, \"OperatorAccount\" BLOB SUB_TYPE TEXT, \"StartTime\" TIMESTAMP, \"EndTime\" TIMESTAMP, \"IPAddress\" BLOB SUB_TYPE TEXT, CONSTRAINT \"PK_RunningLogs\" PRIMARY KEY (\"Id\")); ";

                //执行创建表
                _SqlHelper.ExecuteNonQuery(sql);
            }
        }
        #endregion
    }
}
