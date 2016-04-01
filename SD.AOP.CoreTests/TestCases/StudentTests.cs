﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SD.AOP.CoreTests.StubEntities;

namespace SD.AOP.CoreTests.TestCases
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
    }
}
