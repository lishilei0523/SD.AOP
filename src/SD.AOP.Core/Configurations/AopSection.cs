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
        private static AopSection _Setting;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static AopSection()
        {
            _Setting = null;
        }

        #endregion

        #region # 初始化 —— static void Initialize(Configuration configuration)
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configuration">配置</param>
        public static void Initialize(Configuration configuration)
        {
            #region # 验证

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration), "配置不可为空！");
            }

            #endregion

            _Setting = (AopSection)configuration.GetSection("sd.aop");
        }
        #endregion

        #region # 访问器 —— static AopSection Setting
        /// <summary>
        /// 访问器
        /// </summary>
        public static AopSection Setting
        {
            get
            {
                if (_Setting == null)
                {
                    _Setting = (AopSection)ConfigurationManager.GetSection("sd.aop");
                }
                if (_Setting == null)
                {
                    throw new ApplicationException("SD.AOP节点未配置，请检查程序！");
                }

                return _Setting;
            }
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
