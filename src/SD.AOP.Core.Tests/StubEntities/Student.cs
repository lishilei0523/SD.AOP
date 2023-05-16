using SD.AOP.Core.Aspects.ForMethod;
using SD.AOP.Core.Attributes;
using SD.AOP.Core.Tests.StubImplements;
using System;

namespace SD.AOP.Core.Tests.StubEntities
{
    /// <summary>
    /// 学生
    /// </summary>
    [StubExceptionLogAspect]
    public class Student
    {
        #region # 构造器

        /// <summary>
        /// 创建学生
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="gender">性别</param>
        /// <param name="age">年龄</param>
        [RunningLogAspect]
        public Student(string name, bool gender, int age)
        {
            //验证
            if (string.IsNullOrWhiteSpace(name))
            {
                InvalidOperationException innerException1 = new InvalidOperationException("内部异常第一层");
                InvalidOperationException innerException2 = new InvalidOperationException("内部异常第二层", innerException1);
                InvalidOperationException innerException3 = new InvalidOperationException("内部异常第三层", innerException2);

                ArgumentNullException exception = new ArgumentNullException(@"姓名不可为空！", innerException3);

                throw exception;
            }

            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Gender = gender;
            this.Age = age;
        }

        #endregion

        #region # 属性

        #region 标识Id —— Guid Id
        /// <summary>
        /// 标识Id
        /// </summary>
        public Guid Id { get; set; }
        #endregion

        #region 姓名 —— string Name
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; private set; }
        #endregion

        #region 性别 —— bool Gender
        /// <summary>
        /// 性别
        /// </summary>
        public bool Gender { get; private set; }
        #endregion

        #region 年龄 —— int Age
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; private set; }
        #endregion 

        #endregion

        #region # 方法

        #region 修改学生 —— void UpdateInfo(string name...
        /// <summary>
        /// 修改学生
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="gender">性别</param>
        /// <param name="age">年龄</param>
        [SkipException]
        public void UpdateInfo(string name, bool gender, int age)
        {
            //验证参数
            if (string.IsNullOrWhiteSpace(name))
            {
                InvalidOperationException innerException1 = new InvalidOperationException("内部异常第一层");
                InvalidOperationException innerException2 = new InvalidOperationException("内部异常第二层", innerException1);
                InvalidOperationException innerException3 = new InvalidOperationException("内部异常第三层", innerException2);

                ArgumentNullException exception = new ArgumentNullException(@"姓名不可为空！", innerException3);

                throw exception;
            }

            this.Name = name;
            this.Gender = gender;
            this.Age = age;
        }
        #endregion 

        #endregion
    }
}
