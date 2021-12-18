using ArxOne.MrAdvice.Advice;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SD.AOP.Core.Aspects.ForAny
{
    /// <summary>
    /// 写入文件异常AOP特性
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
            Task.Factory.StartNew(() =>
            {
                this.WriteFile(log, "=============================发生异常, 详细信息如下==================================="
                                    + Environment.NewLine + "［异常时间］" + DateTime.Now
                                    + Environment.NewLine + "［异常消息］" + exception.Message
                                    + Environment.NewLine + "［内部异常］" + exception.InnerException
                                    + Environment.NewLine + "［应用程序］" + exception.Source
                                    + Environment.NewLine + "［当前方法］" + exception.TargetSite
                                    + Environment.NewLine + "［堆栈信息］" + exception.StackTrace
                                    + Environment.NewLine);
            });
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        public void WriteFile(string path, string content)
        {
            #region # 验证

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path), "路径不可为空！");
            }

            #endregion

            FileInfo file = new FileInfo(path);
            StreamWriter writer = null;
            try
            {
                //获取文件目录并判断是否存在
                string directory = Path.GetDirectoryName(path);
                if (string.IsNullOrWhiteSpace(directory))
                {
                    throw new ArgumentNullException(nameof(path), "目录不可为空！");
                }
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                writer = file.AppendText();
                writer.Write(content);
            }
            finally
            {
                writer?.Dispose();
            }
        }
    }
}
