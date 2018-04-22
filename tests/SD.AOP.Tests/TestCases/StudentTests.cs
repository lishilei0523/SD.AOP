using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.AOP.Aspects.ForMethod;
using SD.AOP.Tests.StubEntities;
using System;
using System.Diagnostics;
using System.Transactions;

namespace SD.AOP.Tests.TestCases
{
    [TestClass]
    public class StudentTests
    {
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
