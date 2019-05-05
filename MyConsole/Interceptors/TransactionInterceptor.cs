using System;
using Autofac;
using Castle.DynamicProxy;
using MyWalletLib;

namespace MyConsole.Interceptors
{
    public class TransactionInterceptor : IInterceptor
    {
        private readonly INotification _notify;

        public TransactionInterceptor(INotification notify)
        {
            _notify = notify;
        }

        public void Intercept(IInvocation invocation)
        {
            var transactionAttribute = GetTransactionAttribute(invocation);
            if (transactionAttribute == null)
            {
                invocation.Proceed();
                return;
            }

            using (var transactionScope = Program._container.Resolve<ITransactionScope>())
            {
                try
                {
                    invocation.Proceed();
                    transactionScope.Complete();
                }
                catch (TransactionAbortedException ex)
                {
                    _notify.Push(transactionAttribute.Role, ex.ToString());
                    throw;
                }
            }
        }

        private static TransactionAttribute GetTransactionAttribute(IInvocation invocation)
        {
            return Attribute.GetCustomAttribute(
                invocation.MethodInvocationTarget, typeof(TransactionAttribute)) as TransactionAttribute;
        }
    }
}