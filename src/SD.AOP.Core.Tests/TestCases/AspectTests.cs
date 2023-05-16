using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.AOP.Core.Aspects.ForMethod;
using SD.AOP.Core.Tests.StubEntities;
using SD.Common;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Transactions;

namespace SD.AOP.Core.Tests.TestCases
{
    /// <summary>
    /// 切面测试
    /// </summary>
    [TestClass]
    public class AspectTests
    {
        #region # 测试初始化 —— void Initialize()
        /// <summary>
        /// 测试初始化
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
#if NETCOREAPP3_1_OR_GREATER
            //初始化配置文件
            Assembly entryAssembly = Assembly.GetExecutingAssembly();
            Configuration configuration = ConfigurationExtension.GetConfigurationFromAssembly(entryAssembly);
            AopSection.Initialize(configuration);
#endif
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            AppDomain.CurrentDomain.SetData("DataDirectory", baseDirectory);
        }
        #endregion

        #region # 测试异常日志 —— void TestExceptionLog()
        /// <summary>
        /// 测试异常日志
        /// </summary>
        [TestMethod]
        public void TestExceptionLog()
        {
            for (int i = 0; i < 1000; i++)
            {
                try
                {
                    Student student = new Student(null, true, 20);
                    Trace.WriteLine(student);
                }
                catch (Exception exception)
                {
                    Trace.WriteLine(exception);
                }
            }
        }
        #endregion

        #region # 测试运行日志 —— void TestRunningLog()
        /// <summary>
        /// 测试运行日志
        /// </summary>
        [TestMethod]
        public void TestRunningLog()
        {
            for (int i = 0; i < 1000; i++)
            {
                Student student = new Student("Tom", true, 20);
                Trace.WriteLine(student);
            }
        }
        #endregion

        #region # 测试跳过异常日志 —— void TestSkipException()
        /// <summary>
        /// 测试跳过异常日志
        /// </summary>
        [TestMethod]
        public void TestSkipException()
        {
            for (int i = 0; i < 1000; i++)
            {
                try
                {
                    Student student = new Student("Tom", true, 20);
                    student.UpdateInfo(null, false, 25);
                }
                catch (Exception exception)
                {
                    Trace.WriteLine(exception);
                }
            }
        }
        #endregion

        #region # 测试事务切面 —— void TestTransaction()
        /// <summary>
        /// 测试事务切面
        /// </summary>
        [TestMethod]
        [TransactionAspect(TransactionScopeOption.RequiresNew)]
        public void TestTransaction()
        {
            Trace.WriteLine("OK");
        }
        #endregion
    }
}
