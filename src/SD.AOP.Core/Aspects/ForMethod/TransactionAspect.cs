using ArxOne.MrAdvice.Advice;
using System;
using System.Transactions;

namespace SD.AOP.Core.Aspects.ForMethod
{
    /// <summary>
    /// 事务AOP特性
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class TransactionAspect : Attribute, IMethodAdvice
    {
        /// <summary>
        /// 事务范围选项
        /// </summary>
        private readonly TransactionScopeOption _scopeOption;

        /// <summary>
        /// 基础构造器
        /// </summary>
        /// <param name="scopeOption">事务范围选项</param>
        public TransactionAspect(TransactionScopeOption scopeOption = TransactionScopeOption.Required)
        {
            this._scopeOption = scopeOption;
        }

        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="context">方法元数据</param>
        public void Advise(MethodAdviceContext context)
        {
#if NET40 || NET45
            using (TransactionScope scope = new TransactionScope(this._scopeOption))
#endif
#if NET451_OR_GREATER || NETSTANDARD2_0_OR_GREATER
            using (TransactionScope scope = new TransactionScope(this._scopeOption, TransactionScopeAsyncFlowOption.Enabled))
#endif
            {
                context.Proceed();
                scope.Complete();
            }

        }
    }
}
