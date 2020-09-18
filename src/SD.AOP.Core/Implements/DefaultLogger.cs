using SD.AOP.Core.Interfaces;
using SD.AOP.Core.Models.Entities;
using SD.Common;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace SD.AOP.Core.Implements
{
    /// <summary>
    /// 日志记录者默认实现
    /// </summary>
    public class DefaultLogger : ILoggger
    {
        #region # 常量、字段及构造器

        /// <summary>
        /// 日志数据库连接字符串名称
        /// </summary>
        private const string DefaultConnectionStringName = "LogConnection";

        /// <summary>
        /// SQL工具
        /// </summary>
        private static readonly SqlHelper _SqlHelper;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static DefaultLogger()
        {
            ConnectionStringSettings connectionStringSetting = ConfigurationManager.ConnectionStrings[DefaultConnectionStringName];

            #region # 验证

            if (connectionStringSetting == null)
            {
                throw new NullReferenceException($"未找到name为\"{DefaultConnectionStringName}\"的连接字符串！");
            }

            #endregion

            //初始化SQL工具
            _SqlHelper = new SqlHelper(connectionStringSetting.ConnectionString);
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
            string sql = "INSERT INTO ExceptionLogs (Id, Namespace, ClassName, MethodName, MethodType, ArgsJson, ExceptionType, ExceptionMessage, ExceptionInfo, InnerException, OccurredTime, IPAddress) OUTPUT inserted.Id VALUES (NEWID(), @Namespace, @ClassName, @MethodName, @MethodType, @ArgsJson, @ExceptionType, @ExceptionMessage, @ExceptionInfo, @InnerException, @OccurredTime, @IPAddress)";

            //02.初始化参数
            SqlParameter[] parameters = {
                    new SqlParameter("@Namespace",log.Namespace.ToDbValue()),
                    new SqlParameter("@ClassName", log.ClassName.ToDbValue()),
                    new SqlParameter("@MethodName", log.MethodName.ToDbValue()),
                    new SqlParameter("@MethodType", log.MethodType.ToDbValue()),
                    new SqlParameter("@ArgsJson", log.ArgsJson.ToDbValue()),
                    new SqlParameter("@ExceptionType", log.ExceptionType.ToDbValue()),
                    new SqlParameter("@ExceptionMessage", log.ExceptionMessage.ToDbValue()),
                    new SqlParameter("@ExceptionInfo", log.ExceptionInfo.ToDbValue()),
                    new SqlParameter("@InnerException", log.InnerException.ToDbValue()),
                    new SqlParameter("@OccurredTime", log.OccurredTime.ToDbValue()),
                    new SqlParameter("@IPAddress", log.IPAddress.ToDbValue())
                };

            //03.执行sql
            Guid newId = (Guid)_SqlHelper.ExecuteScalar(sql, parameters);
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
            string sql = "INSERT INTO RunningLogs (Id, Namespace, ClassName, MethodName, MethodType, ArgsJson, ReturnValue, ReturnValueType, OperatorAccount, StartTime, EndTime, IPAddress) OUTPUT inserted.Id VALUES (NEWID(), @Namespace, @ClassName, @MethodName, @MethodType, @ArgsJson, @ReturnValue, @ReturnValueType, @OperatorAccount, @StartTime, @EndTime, @IPAddress)";

            //02.初始化参数
            SqlParameter[] parameters = {
                    new SqlParameter("@Namespace", log.Namespace.ToDbValue()),
                    new SqlParameter("@ClassName", log.ClassName.ToDbValue()),
                    new SqlParameter("@MethodName", log.MethodName.ToDbValue()),
                    new SqlParameter("@MethodType", log.MethodType.ToDbValue()),
                    new SqlParameter("@ArgsJson", log.ArgsJson.ToDbValue()),
                    new SqlParameter("@ReturnValue", log.ReturnValue.ToDbValue()),
                    new SqlParameter("@ReturnValueType", log.ReturnValueType.ToDbValue()),
                    new SqlParameter("@OperatorAccount", log.OperatorAccount.ToDbValue()),
                    new SqlParameter("@StartTime", log.StartTime.ToDbValue()),
                    new SqlParameter("@EndTime", log.EndTime.ToDbValue()),
                    new SqlParameter("@IPAddress", log.IPAddress.ToDbValue())
                };

            //03.执行sql
            Guid newId = (Guid)_SqlHelper.ExecuteScalar(sql, parameters);
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
            sqlBuilder.Append("IF OBJECT_ID('[dbo].[ExceptionLogs]') IS NULL ");
            sqlBuilder.Append("BEGIN ");
            sqlBuilder.Append("CREATE TABLE [dbo].[ExceptionLogs] ([Id] [uniqueidentifier] PRIMARY KEY NOT NULL, [Namespace] [nvarchar](max) NULL, [ClassName] [nvarchar](max) NULL, [MethodName] [nvarchar](max) NULL, [MethodType] [nvarchar](max) NULL, [ArgsJson] [nvarchar](max) NULL, [ExceptionType] [nvarchar](max) NULL, [ExceptionMessage] [nvarchar](max) NULL, [ExceptionInfo] [nvarchar](max) NULL, [InnerException] [nvarchar](max) NULL, [OccurredTime] [datetime] NULL, [IPAddress] [nvarchar](max) NULL) ");
            sqlBuilder.Append("END ");

            //初始化程序运行日志
            sqlBuilder.Append("IF OBJECT_ID('[dbo].[RunningLogs]') IS NULL ");
            sqlBuilder.Append("BEGIN ");
            sqlBuilder.Append("CREATE TABLE [dbo].[RunningLogs] ([Id] [uniqueidentifier] PRIMARY KEY NOT NULL, [Namespace] [nvarchar](max) NULL, [ClassName] [nvarchar](max) NULL, [MethodName] [nvarchar](max) NULL, [MethodType] [nvarchar](max) NULL, [ArgsJson] [nvarchar](max) NULL, [ReturnValue] [nvarchar](max) NULL, [ReturnValueType] [nvarchar](max) NULL, [OperatorAccount] [nvarchar](max) NULL, [StartTime] [datetime] NULL, [EndTime] [datetime] NULL, [IPAddress] [nvarchar](max) NULL) ");
            sqlBuilder.Append("END ");

            //执行创建表
            _SqlHelper.ExecuteNonQuery(sqlBuilder.ToString());
        }
        #endregion
    }
}
