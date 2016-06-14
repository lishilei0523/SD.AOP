using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace SD.AOP.Core.Toolkits
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class Common
    {
        #region # 获取本机IP地址 —— static string GetLocalIPAddress()
        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <returns>本机IP</returns>
        public static string GetLocalIPAddress()
        {
            StringBuilder buid = new StringBuilder();

            string hostName = Dns.GetHostName();//本机名   
            buid.Append(hostName + ";");
            IPAddress[] addressList = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6   

            foreach (IPAddress ip in addressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    buid.Append(ip + ";");
                }
            }
            return buid.ToString();
        }
        #endregion

        #region # object序列化JSON字符串扩展方法 —— static string ToJson(this object obj)
        /// <summary>
        /// object序列化JSON字符串扩展方法
        /// </summary>
        /// <param name="obj">object及其子类对象</param>
        /// <returns>JSON字符串</returns>
        public static string ToJson(this object obj)
        {
            #region # 验证参数

            if (obj == null)
            {
                return null;
            }

            #endregion

            try
            {
                JsonSerializerSettings settting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
                return JsonConvert.SerializeObject(obj, Formatting.None, settting);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
        #endregion

        #region # C#值转数据库值空值处理 —— static object ToDbValue(this object value)
        /// <summary>
        /// C#值转数据库值空值处理
        /// </summary>
        /// <param name="value">C#值</param>
        /// <returns>处理后的数据库值</returns>
        public static object ToDbValue(this object value)
        {
            return value ?? DBNull.Value;
        }
        #endregion

        #region # 填充 —— static void Fill(TSource sourceInstance, TTarget targetInstance)
        /// <summary>
        /// 将两个对象名称相同的属性替换赋值
        /// </summary>
        /// <param name="sourceInstance">源实例</param>
        /// <param name="targetInstance">目标实例</param>
        public static void Fill<TSource, TTarget>(this TSource sourceInstance, TTarget targetInstance)
        {
            //01.获取源对象与目标对象的类型
            Type sourceType = sourceInstance.GetType();
            Type targetType = targetInstance.GetType();

            //02.获取源对象与目标对象的所有属性
            PropertyInfo[] sourceProps = sourceType.GetProperties();
            PropertyInfo[] targetProps = targetType.GetProperties();

            //03.双重遍历，判断属性名称是否相同，如果相同则赋值
            foreach (PropertyInfo tgtProp in targetProps)
            {
                foreach (PropertyInfo srcProp in sourceProps)
                {
                    if (tgtProp.Name == srcProp.Name)
                    {
                        tgtProp.SetValue(targetInstance, srcProp.GetValue(sourceInstance, null), null);
                    }
                }
            }
        }
        #endregion
    }
}
