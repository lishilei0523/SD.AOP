using System.Configuration;

namespace SD.AOP.Core.Configurations
{
    /// <summary>
    /// 连接字符串配置节点
    /// </summary>
    public class ConnectionStringElement : ConfigurationElement
    {
        #region # 名称 —— string Name
        /// <summary>
        /// 名称
        /// </summary>
        [ConfigurationProperty("name", IsRequired = false)]
        public string Name
        {
            get { return this["name"].ToString(); }
            set { this["name"] = value; }
        }
        #endregion

        #region # 值 —— string Value
        /// <summary>
        /// 值
        /// </summary>
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return this["value"].ToString(); }
            set { this["value"] = value; }
        }
        #endregion
    }
}
