using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.Common;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace SD.AOP.Core.Tests.TestCases
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestInitialize]
        public void Init()
        {
#if NETCOREAPP3_1_OR_GREATER
            //初始化配置文件
            Assembly entryAssembly = Assembly.GetExecutingAssembly();
            Configuration configuration = ConfigurationExtension.GetConfigurationFromAssembly(entryAssembly);
            AopSection.Initialize(configuration);
#endif
        }

        [TestMethod]
        public void TestConfigurations()
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
    }
}
