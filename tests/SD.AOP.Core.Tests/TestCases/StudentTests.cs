using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.AOP.Core.Aspects.ForMethod;
using SD.AOP.Core.Tests.StubEntities;
using System;
using System.Diagnostics;
using System.Transactions;

namespace SD.AOP.Core.Tests.TestCases
{
    [TestClass]
    public class StudentTests
    {
        /// <summary>
        /// 创建学生测试 - 测试异常日志
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateStudentTest_TestExceptionLog()
        {
            Student student = new Student(null, true, 20);
        }

        /// <summary>
        /// 创建学生测试 - 测试运行日志
        /// </summary>
        [TestMethod]
        public void CreateStudentTest_TestRunningLog()
        {
            Student student = new Student("Tom", true, 20);
        }

        /// <summary>
        /// 修改学生测试 - 测试跳过异常日志
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateStudentTest_TestSkipException()
        {
            Student student = new Student("Tom", true, 20);
            student.UpdateInfo(Guid.NewGuid(), null, false, 25);
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
