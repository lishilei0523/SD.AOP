using System;

namespace SD.AOP.Core.Toolkits
{
    /// <summary>
    /// 通用扩展
    /// </summary>
    public static class CommomExtension
    {
        #region # CLS类型值转数据库类型值 —— static object ToDbValue(this object value)
        /// <summary>
        /// CLS类型值转数据库类型值
        /// </summary>
        /// <param name="value">CLS类型值</param>
        /// <returns>数据库类型值</returns>
        public static object ToDbValue(this object value)
        {
            return value ?? DBNull.Value;
        }
        #endregion
    }
}
