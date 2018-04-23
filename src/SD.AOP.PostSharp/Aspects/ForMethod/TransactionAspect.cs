﻿using PostSharp.Aspects;
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
        /// 基础构造器
        /// </summary>
        /// <param name="scopeOption">事务范围选项</param>
        public TransactionAspect(TransactionScopeOption scopeOption = TransactionScopeOption.Required)
        {
            this._scopeOption = scopeOption;
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
            TransactionScopeAsyncFlowOption asyncFlowOption = TransactionScopeAsyncFlowOption.Enabled;

            using (TransactionScope scope = new TransactionScope(this._scopeOption, asyncFlowOption))
            {
                base.OnInvoke(eventArgs);
                scope.Complete();
            }
        }
    }
}