using PostSharp.Aspects;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SD.AOP.Core.Aspects.ForAny
{
    /// <summary>
    /// 写入文件异常AOP特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class WriteFileExceptionAspect : OnExceptionAspect
    {
        /// <summary>
        /// 异常过滤器
        /// </summary>
        /// <param name="eventArgs">方法元数据</param>
        public override void OnException(MethodExecutionArgs eventArgs)
        {
            //日志记录到文件中
            string log = $"{AppDomain.CurrentDomain.BaseDirectory}Logs\\Log_{DateTime.Now.Date:yyyyMMdd}.txt";

            Task.Run(() =>
            {
                this.WriteFile(log, "=============================发生异常, 详细信息如下==================================="
                                    + Environment.NewLine + "［异常时间］" + DateTime.Now
                                    + Environment.NewLine + "［异常消息］" + eventArgs.Exception.Message
                                    + Environment.NewLine + "［内部异常］" + eventArgs.Exception.InnerException
                                    + Environment.NewLine + "［应用程序］" + eventArgs.Exception.Source
                                    + Environment.NewLine + "［当前方法］" + eventArgs.Exception.TargetSite
                                    + Environment.NewLine + "［堆栈信息］" + eventArgs.Exception.StackTrace
                                    + Environment.NewLine, true);
            });

            base.OnException(eventArgs);
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
