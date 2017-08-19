using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SD.AOP.LogSite.Repositories
{
    /// <summary>
    /// 工具
    /// </summary>
    public static class Common
    {
        #region # 数据库值转C#值空值处理 —— static object ToNetValue(IDataReader reader...
        /// <summary>
        /// 数据库值转C#值空值处理
        /// </summary>
        /// <param name="reader">IDataReader对象</param>
        /// <param name="columnName">列名</param>
        /// <returns>C#值</returns>
        public static object ToNetValue(IDataReader reader, string columnName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(columnName)))
            {
                return null;
            }

            return reader[columnName];
        }

        #endregion

        #region # 获取Id列表字符串 —— static string GetIdsString(IEnumerable<Guid> ids)
        /// <summary>
        /// 获取Id列表字符串
        /// </summary>
        /// <param name="ids">Id列表</param>
        /// <returns>Id列表字符串</returns>
        public static string GetIdsString(IEnumerable<Guid> ids)
        {
            StringBuilder builder = new StringBuilder();

            foreach (Guid id in ids)
            {
                builder.Append(id);
                builder.Append(',');
            }
            if (builder.Length == 0)
            {
                return string.Empty;
            }

            return builder.ToString().Substring(0, builder.Length - 1);
        }
        #endregion
    }
}