using PostSharp.Aspects;
using SD.AOP.Core.Interfaces;
using SD.AOP.Core.Models.Entities;
using SD.AOP.Core.Toolkits;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;

namespace SD.AOP.Core.Aspects.ForMethod
{
    /// <summary>
    /// 程序日志AOP特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true)]
    public class RunningLogAspect : OnMethodBoundaryAspect
    {
        /// <summary>
        /// 程序日志字段
        /// </summary>
        protected readonly RunningLog _runningLog;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RunningLogAspect()
        {
            this._runningLog = new RunningLog();
        }

        /// <summary>
        /// 执行方法开始事件
        /// </summary>
        /// <param name="eventArgs">方法元数据</param>
        public override void OnEntry(MethodExecutionArgs eventArgs)
        {
            this._runningLog.BuildRuningInfo(eventArgs);
            this._runningLog.BuildBasicInfo(eventArgs);
            this._runningLog.BuildMethodArgsInfo(eventArgs);
        }

        /// <summary>
        /// 执行方法结束事件
        /// </summary>
        /// <param name="eventArgs">方法元数据</param>
        public override void OnExit(MethodExecutionArgs eventArgs)
        {
            this._runningLog.BuildReturnValueInfo(eventArgs);

            //读取配置文件实例化日志记录提供者
            Assembly impAssembly = Assembly.Load(LoggerProviderConfiguration.Setting.Assembly);
            Type implType = impAssembly.GetType(LoggerProviderConfiguration.Setting.Type);
            ILoggger loggger = (ILoggger)Activator.CreateInstance(implType);

            //无需事务
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                //持久化
                Task.Run(() => loggger.Write(this._runningLog));

                scope.Complete();
            }
        }
    }
}