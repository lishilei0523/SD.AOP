using System;

namespace SD.AOP.Core.Models.ValueObjects
{
    /// <summary>
    /// 方法参数
    /// </summary>
    [Serializable]
    internal struct MethodArg
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="type">参数类型</param>
        /// <param name="value">参数值</param>
        public MethodArg(string name, string type, object value)
        {
            this.Name = name;
            this.Type = type;
            this.Value = value;
        }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public object Value { get; set; }
    }
}
