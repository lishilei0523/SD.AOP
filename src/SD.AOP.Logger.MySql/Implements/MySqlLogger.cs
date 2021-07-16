using MySql.Data.MySqlClient;
using SD.AOP.Core;
using SD.AOP.Core.Interfaces;
using SD.AOP.Core.Models.Entities;
using SD.AOP.Core.Toolkits;
using System;
using System.Configuration;
using System.Data;
using System.Text;

// ReSharper disable once CheckNamespace
namespace SD.AOP
{
    /// <summary>
    /// MySQL日志记录者实现
    /// </summary>
    public class MySqlLogger : ILoggger
    {
        #region # 常量、字段及构造器

        /// <summary>
        /// SQL工具
        /// </summary>
        private static readonly SD.Toolkits.Sql.MySql.MySqlHelper _SqlHelper;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static MySqlLogger()
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
            _SqlHelper = new SD.Toolkits.Sql.MySql.MySqlHelper(connectionString);
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

            //01.构造sql语句
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SET @NEWID = UUID(); ");
            sqlBuilder.Append(
                "INSERT INTO ExceptionLogs (Id, Namespace, ClassName, MethodName, MethodType, ArgsJson, ExceptionType, ExceptionMessage, ExceptionInfo, InnerException, OccurredTime, IPAddress) VALUES (@NEWID, @Namespace, @ClassName, @MethodName, @MethodType, @ArgsJson, @ExceptionType, @ExceptionMessage, @ExceptionInfo, @InnerException, @OccurredTime, @IPAddress); ");
            sqlBuilder.Append("SELECT @NEWID; ");

            //02.初始化参数
            IDbDataParameter[] parameters = {
                    new MySqlParameter("@Namespace",log.Namespace.ToDbValue()),
                    new MySqlParameter("@ClassName", log.ClassName.ToDbValue()),
                    new MySqlParameter("@MethodName", log.MethodName.ToDbValue()),
                    new MySqlParameter("@MethodType", log.MethodType.ToDbValue()),
                    new MySqlParameter("@ArgsJson", log.ArgsJson.ToDbValue()),
                    new MySqlParameter("@ExceptionType", log.ExceptionType.ToDbValue()),
                    new MySqlParameter("@ExceptionMessage", log.ExceptionMessage.ToDbValue()),
                    new MySqlParameter("@ExceptionInfo", log.ExceptionInfo.ToDbValue()),
                    new MySqlParameter("@InnerException", log.InnerException.ToDbValue()),
                    new MySqlParameter("@OccurredTime", log.OccurredTime.ToDbValue()),
                    new MySqlParameter("@IPAddress", log.IPAddress.ToDbValue())
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
            //初始化日志数据表
            InitTable();

            //01.构造SQL语句
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SET @NEWID = UUID(); ");
            sqlBuilder.Append("INSERT INTO RunningLogs (Id, Namespace, ClassName, MethodName, MethodType, ArgsJson, ReturnValue, ReturnValueType, OperatorAccount, StartTime, EndTime, IPAddress) VALUES (@NEWID, @Namespace, @ClassName, @MethodName, @MethodType, @ArgsJson, @ReturnValue, @ReturnValueType, @OperatorAccount, @StartTime, @EndTime, @IPAddress); ");
            sqlBuilder.Append("SELECT @NEWID; ");

            //02.初始化参数
            IDbDataParameter[] parameters = {
                    new MySqlParameter("@Namespace", log.Namespace.ToDbValue()),
                    new MySqlParameter("@ClassName", log.ClassName.ToDbValue()),
                    new MySqlParameter("@MethodName", log.MethodName.ToDbValue()),
                    new MySqlParameter("@MethodType", log.MethodType.ToDbValue()),
                    new MySqlParameter("@ArgsJson", log.ArgsJson.ToDbValue()),
                    new MySqlParameter("@ReturnValue", log.ReturnValue.ToDbValue()),
                    new MySqlParameter("@ReturnValueType", log.ReturnValueType.ToDbValue()),
                    new MySqlParameter("@OperatorAccount", log.OperatorAccount.ToDbValue()),
                    new MySqlParameter("@StartTime", log.StartTime.ToDbValue()),
                    new MySqlParameter("@EndTime", log.EndTime.ToDbValue()),
                    new MySqlParameter("@IPAddress", log.IPAddress.ToDbValue())
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

            sqlBuilder.Append("CREATE TABLE IF NOT EXISTS `ExceptionLogs` (`Id` char(36) NOT NULL, `Namespace` longtext default NULL, `ClassName` longtext default NULL, `MethodName` longtext default NULL, `MethodType` longtext default NULL, `ArgsJson` longtext default NULL, `ExceptionType` longtext default NULL, `ExceptionMessage` longtext default NULL, `ExceptionInfo` longtext default NULL, `InnerException` longtext default NULL, `OccurredTime` datetime default NULL, `IPAddress` longtext default NULL, PRIMARY KEY (`Id`)); ");

            //初始化程序运行日志
            sqlBuilder.Append("CREATE TABLE IF NOT EXISTS `RunningLogs` (`Id` char(36) NOT NULL, `Namespace` longtext NULL, `ClassName` longtext NULL, `MethodName` longtext NULL, `MethodType` longtext NULL, `ArgsJson` longtext NULL, `ReturnValue` longtext NULL, `ReturnValueType` longtext NULL, `OperatorAccount` longtext NULL, `StartTime` datetime NULL, `EndTime` datetime NULL, `IPAddress` longtext NULL, PRIMARY KEY (`Id`)); ");

            //执行创建表
            _SqlHelper.ExecuteNonQuery(sqlBuilder.ToString());
        }
        #endregion
    }
}
