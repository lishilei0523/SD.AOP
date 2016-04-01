﻿using System;
using SD.AOP.Core.Aspects.ForMethod;
using SD.AOP.CoreTests.StubImplements;

namespace SD.AOP.CoreTests.StubEntities
{
    /// <summary>
    /// 学生
    /// </summary>
    [StubExceptionLogAspect]                    //记录异常日志标签
    public class Student
    {
        /// <summary>
        /// 创建学生
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="gender">性别</param>
        /// <param name="age">年龄</param>
        [RunningLogAspect]                      //记录运行日志标签
        public Student(string name, bool gender, int age)
        {
            //验证参数

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name", @"姓名不可为空！");
            }

            this.Name = name;
            this.Gender = gender;
            this.Age = age;
        }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 性别
        /// </summary>
        public bool Gender { get; private set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; private set; }
    }
}