using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Common;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace SD.AOP.Core.Tests.TestCases
{
    /// <summary>
    /// 配置文件测试
    /// </summary>
    [TestClass]
    public class ConfigurationTests
    {
        #region # 测试初始化 —— void Initialize()
        /// <summary>
        /// 测试初始化
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
#if NETCOREAPP3_1_OR_GREATER
            Assembly entryAssembly = Assembly.GetExecutingAssembly();
            Configuration configuration = ConfigurationExtension.GetConfigurationFromAssembly(entryAssembly);
            AopSection.Initialize(configuration);
#endif
        }
        #endregion

        #region # 测试配置文件 —— void TestConfiguration()
        /// <summary>
        /// 测试配置文件
        /// </summary>
        [TestMethod]
        public void TestConfiguration()
        {
            string loggerProviderType = AopSection.Setting.LoggerProvider.Type;
            string loggerProviderAssembly = AopSection.Setting.LoggerProvider.Assembly;
            string connectionStringName = AopSection.Setting.ConnectionString.Name;
            string connectionString = AopSection.Setting.ConnectionString.Value;

            Trace.WriteLine(loggerProviderType);
            Trace.WriteLine(loggerProviderAssembly);
            Trace.WriteLine(connectionStringName);
            Trace.WriteLine(connectionString);
        }
        #endregion
    }
}
