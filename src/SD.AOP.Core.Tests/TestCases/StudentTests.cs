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
    [TestClass]
    public class StudentTests
    {
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

        /// <summary>
        /// 创建学生测试 - 测试异常日志
        /// </summary>
        [TestMethod]
        public void CreateStudentTest_TestExceptionLog()
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

        /// <summary>
        /// 创建学生测试 - 测试运行日志
        /// </summary>
        [TestMethod]
        public void CreateStudentTest_TestRunningLog()
        {
            for (int i = 0; i < 1000; i++)
            {
                Student student = new Student("Tom", true, 20);
                Trace.WriteLine(student);
            }
        }

        /// <summary>
        /// 修改学生测试 - 测试跳过异常日志
        /// </summary>
        [TestMethod]
        public void UpdateStudentTest_TestSkipException()
        {
            for (int i = 0; i < 1000; i++)
            {
                try
                {
                    Student student = new Student("Tom", true, 20);
                    student.UpdateInfo(Guid.NewGuid(), null, false, 25);
                }
                catch (Exception exception)
                {
                    Trace.WriteLine(exception);
                }
            }
        }

        /// <summary>
        /// 事务测试
        /// </summary>
        [TestMethod]
        [TransactionAspect(TransactionScopeOption.RequiresNew)]
        public void TransactionTest()
        {
            Trace.WriteLine("OK");
        }
    }
}
