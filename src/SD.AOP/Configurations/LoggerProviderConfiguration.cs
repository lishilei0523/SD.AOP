using System;
using System.Configuration;

// ReSharper disable once CheckNamespace
namespace SD.AOP
{
    /// <summary>
    /// 日志记录者配置
    /// </summary>
    public class LoggerProviderConfiguration : ConfigurationSection
    {
        #region # 字段及构造器

        /// <summary>
        /// 单例
        /// </summary>
        private static readonly LoggerProviderConfiguration _Setting;

        /// <summary>
        /// 静态构造器
        /// </summary>
        static LoggerProviderConfiguration()
        {
            LoggerProviderConfiguration._Setting = (LoggerProviderConfiguration)ConfigurationManager.GetSection("loggerProviderConfiguration");

            #region # 非空验证

            if (LoggerProviderConfiguration._Setting == null)
            {
                throw new ApplicationException("日志记录者节点未配置，请检查程序！");
            }

            #endregion
        }

        #endregion

        #region # 访问器 —— static LoggerProviderConfiguration Setting
        /// <summary>
        /// 访问器
        /// </summary>
        public static LoggerProviderConfiguration Setting
        {
            get { return LoggerProviderConfiguration._Setting; }
        }
        #endregion

        #region # 类型 —— string Type
        /// <summary>
        /// 类型
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get { return this["type"].ToString(); }
            set { this["type"] = value; }
        }
        #endregion

        #region # 程序集 —— string Assembly
        /// <summary>
        /// 程序集
        /// </summary>
        [ConfigurationProperty("assembly", IsRequired = true)]
        public string Assembly
        {
            get { return this["assembly"].ToString(); }
            set { this["assembly"] = value; }
        }
        #endregion
    }
}
