﻿using System;
using System.Data;
using System.Data.SqlClient;

namespace SD.AOP.Logger.SqlServer.Implements
{
    /// <summary>
    /// SQL Server数据库访问助手类
    /// </summary>
    public sealed class SqlServerHelper
    {
        #region # 字段及构造器

        /// <summary>
        /// 连接字符串字段
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SqlServerHelper(string connectionString)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), @"连接字符串不可为空！");
            }

            #endregion

            this._connectionString = connectionString;
        }

        #endregion


        //Public

        #region # 执行SQL语句命令 —— int ExecuteNonQuery(string sql, params SqlParameter[] args)
        /// <summary>
        /// ExecuteNonQuery —— Sql语句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="args">参数</param>
        /// <returns>受影响的行数</returns>
        public int ExecuteNonQuery(string sql, params SqlParameter[] args)
        {
            return this.ExecuteNonQuery(sql, CommandType.Text, args);
        }
        #endregion

        #region # 执行SQL语句返回首行首列值 —— object ExecuteScalar(string sql, params SqlParameter[] args)
        /// <summary>
        /// ExecuteScalar —— Sql语句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="args">参数</param>
        /// <returns>object对象</returns>
        public object ExecuteScalar(string sql, params SqlParameter[] args)
        {
            return this.ExecuteScalar(sql, CommandType.Text, args);
        }
        #endregion


        //Private

        #region # 创建连接方法 —— SqlConnection CreateConnection()
        /// <summary>
        /// 创建连接方法
        /// </summary>
        /// <returns>连接对象</returns>
        private SqlConnection CreateConnection()
        {
            return new SqlConnection(this._connectionString);
        }
        #endregion

        #region # ExecuteNonQuery方法 —— int ExecuteNonQuery(string sql, CommandType type, params SqlParameter[] args)
        /// <summary>
        /// ExecuteNonQuery方法
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="type">命令类型</param>
        /// <param name="args">参数</param>
        /// <returns>受影响的行数</returns>
        private int ExecuteNonQuery(string sql, CommandType type, params SqlParameter[] args)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql), @"SQL语句不可为空！");
            }

            #endregion

            int rowCount;
            using (SqlConnection conn = this.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(sql, conn) { CommandType = type };
                cmd.Parameters.AddRange(args);
                conn.Open();
                rowCount = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            return rowCount;
        }
        #endregion

        #region # ExecuteScalar方法 —— object ExecuteScalar(string sql, CommandType type, params SqlParameter[] args)
        /// <summary>
        /// ExecuteScalar方法
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="type">命令类型</param>
        /// <param name="args">参数</param>
        /// <returns>object对象</returns>
        private object ExecuteScalar(string sql, CommandType type, params SqlParameter[] args)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql), @"SQL语句不可为空！");
            }

            #endregion

            object obj;
            using (SqlConnection conn = this.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(sql, conn) { CommandType = type };
                cmd.Parameters.AddRange(args);
                conn.Open();
                obj = cmd.ExecuteScalar();
                cmd.Dispose();
            }
            return obj;
        }
        #endregion
    }
}
