using System;
using System.Linq;
using Castle.DynamicProxy;
using MyWalletLib;

namespace MyConsole.Interceptors
{
    public class CacheInterceptor : IInterceptor
    {
        private readonly ICacheProvider _cache;

        public CacheInterceptor(ICacheProvider cache)
        {
            _cache = cache;
        }

        public void Intercept(IInvocation invocation)
        {
            var cacheAttribute = GetCacheAttribute(invocation);

            if (cacheAttribute == null)
            {
                invocation.Proceed();
                return;
            }

            var key = GetInvocationSignature(invocation);

            if (_cache.Contains(key))
            {
                invocation.ReturnValue = _cache.Get(key);
                return;
            }

            invocation.Proceed();
            var result = invocation.ReturnValue;

            if (result != null)
            {
                _cache.Put(key, result, cacheAttribute.Duration);
            }
        }

        private CacheAttribute GetCacheAttribute(IInvocation invocation)
        {
            return Attribute.GetCustomAttribute(
                    invocation.MethodInvocationTarget, typeof(CacheAttribute)) as CacheAttribute;
        }

        private string GetInvocationSignature(IInvocation invocation)
        {
            return
                $"{invocation.TargetType.FullName}-{invocation.Method.Name}-{String.Join("-", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())}";
        }
    }
}