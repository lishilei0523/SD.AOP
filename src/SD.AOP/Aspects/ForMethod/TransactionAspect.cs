using ArxOne.MrAdvice.Advice;
using System;
using System.Transactions;

namespace SD.AOP.Aspects.ForMethod
{
    /// <summary>
    /// 事务AOP特性类
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
            TransactionScopeAsyncFlowOption asyncFlowOption = TransactionScopeAsyncFlowOption.Enabled;

            using (TransactionScope scope = new TransactionScope(this._scopeOption, asyncFlowOption))
            {
                context.Proceed();
                scope.Complete();
            }
        }
    }
}
