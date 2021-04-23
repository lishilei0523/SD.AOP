using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace SD.AOP.Core.Tests.TestCases
{
    [TestClass]
    public class ConfigurationTests
    {
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
