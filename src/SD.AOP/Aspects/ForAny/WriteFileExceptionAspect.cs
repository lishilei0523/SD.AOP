using ArxOne.MrAdvice.Advice;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SD.AOP.Aspects.ForAny
{
    /// <summary>
    /// 写入文件异常AOP特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class WriteFileExceptionAspect : ExceptionAspect
    {
        /// <summary>
        /// 发生异常事件
        /// </summary>
        /// <param name="context">方法元数据</param>
        /// <param name="exception">异常实例</param>
        protected override void OnException(MethodAdviceContext context, Exception exception)
        {
            //日志记录到文件中
            string log = $"{AppDomain.CurrentDomain.BaseDirectory}Logs\\Log_{DateTime.Now.Date:yyyyMMdd}.txt";

            Task.Run(() =>
            {
                this.WriteFile(log, "=============================发生异常, 详细信息如下==================================="
                                    + Environment.NewLine + "［异常时间］" + DateTime.Now
                                    + Environment.NewLine + "［异常消息］" + exception.Message
                                    + Environment.NewLine + "［内部异常］" + exception.InnerException
                                    + Environment.NewLine + "［应用程序］" + exception.Source
                                    + Environment.NewLine + "［当前方法］" + exception.TargetSite
                                    + Environment.NewLine + "［堆栈信息］" + exception.StackTrace
                                    + Environment.NewLine, true);
            });
        }

        /// <summary>
        /// 写入文件方法
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        /// <param name="append">是否附加</param>
        /// <exception cref="ArgumentNullException">路径为空</exception>
        public void WriteFile(string path, string content, bool append = false)
        {
            #region # 验证参数

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path), @"路径不可为空！");
            }

            #endregion

            FileInfo file = new FileInfo(path);
            StreamWriter writer = null;
            if (file.Exists && !append)
            {
                file.Delete();
            }
            try
            {
                //获取文件目录并判断是否存在
                string directory = Path.GetDirectoryName(path);

                if (string.IsNullOrEmpty(directory))
                {
                    throw new ArgumentNullException(nameof(path), "目录不可为空！");
                }
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                writer = append ? file.AppendText() : new StreamWriter(path, false, Encoding.UTF8);
                writer.Write(content);
            }
            finally
            {
                writer?.Dispose();
            }
        }
    }
}
