using PostSharp.Aspects;
using PostSharp.Aspects.Configuration;
using System;
using System.Transactions;

namespace SD.AOP.Core.Aspects.ForMethod
{
    /// <summary>
    /// 事务AOP特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class TransactionAspect : MethodInterceptionAspect
    {
        /// <summary>
        /// 事务范围选项
        /// </summary>
        private readonly TransactionScopeOption _scopeOption;

        /// <summary>
        /// 事务选项
        /// </summary>
        private readonly TransactionOptions _transactionOptions;

        /// <summary>
        /// 基础构造器
        /// </summary>
        /// <param name="scopeOption">事务范围选项</param>
        /// <param name="isolationLevel">隔离级别</param>
        public TransactionAspect(TransactionScopeOption scopeOption = TransactionScopeOption.Required, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            this._scopeOption = scopeOption;
            this._transactionOptions = new TransactionOptions
            {
                IsolationLevel = isolationLevel
            };
        }

        /// <summary>
        /// 重写方面配置
        /// </summary>
        /// <returns>方面配置</returns>
        protected override AspectConfiguration CreateAspectConfiguration()
        {
            base.AspectPriority = 100;
            return base.CreateAspectConfiguration();
        }

        /// <summary>
        /// 重写当方法被调用时执行事件
        /// </summary>
        /// <param name="eventArgs">方法元数据</param>
        public override void OnInvoke(MethodInterceptionArgs eventArgs)
        {
            using (TransactionScope scope = new TransactionScope(this._scopeOption, this._transactionOptions))
            {
                base.OnInvoke(eventArgs);
                scope.Complete();
            }
        }
    }
}
