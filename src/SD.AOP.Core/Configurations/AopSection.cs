using SD.AOP.Core.Configurations;
using System;
using System.Configuration;

// ReSharper disable once CheckNamespace
namespace SD.AOP.Core
{
    /// <summary>
    /// SD.AOP配置
    /// </summary>
    public class AopSection : ConfigurationSection
    {
        #region # 字段及构造器

        /// <summary>
        /// 单例
        /// </summary>
        private static readonly AopSection _Setting;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static AopSection()
        {
            _Setting = (AopSection)ConfigurationManager.GetSection("sd.aop");

            #region # 非空验证

            if (_Setting == null)
            {
                throw new ApplicationException("日志记录者节点未配置，请检查程序！");
            }

            #endregion
        }

        #endregion

        #region # 访问器 —— static AopSection Setting
        /// <summary>
        /// 访问器
        /// </summary>
        public static AopSection Setting
        {
            get { return _Setting; }
        }
        #endregion

        #region # 日志提供者节点 —— LoggerProviderElement LoggerProvider
        /// <summary>
        /// 日志提供者节点
        /// </summary>
        [ConfigurationProperty("loggerProvider", IsRequired = true)]
        public LoggerProviderElement LoggerProvider
        {
            get { return (LoggerProviderElement)this["loggerProvider"]; }
            set { this["loggerProvider"] = value; }
        }
        #endregion

        #region # 连接字符串节点 —— ConnectionStringElement ConnectionString
        /// <summary>
        /// 连接字符串节点
        /// </summary>
        [ConfigurationProperty("connectionString", IsRequired = false)]
        public ConnectionStringElement ConnectionString
        {
            get { return (ConnectionStringElement)this["connectionString"]; }
            set { this["connectionString"] = value; }
        }
        #endregion
    }
}
