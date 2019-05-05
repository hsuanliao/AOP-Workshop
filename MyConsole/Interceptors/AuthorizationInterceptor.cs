using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using MyWalletLib;

namespace MyConsole.Interceptors
{
    internal class AuthorizationInterceptor : IInterceptor
    {
        private readonly IContext _context;

        public AuthorizationInterceptor(IContext context)
        {
            _context = context;
        }

        public void Intercept(IInvocation invocation)
        {
            var authorizedAttributes = GetAuthorizedAttributes(invocation);
            if (authorizedAttributes.Any())
            {
                var currentUser = _context.GetCurrentUser();
                if (!authorizedAttributes.Select(a => a.UserType).Contains(currentUser.UserType))
                {
                    throw new UnauthorizedAccessException(
                        $"{currentUser.Id} is {currentUser.UserType} without authorization for {invocation.TargetType.FullName}.{invocation.Method.Name}()");
                }
            }

            invocation.Proceed();
        }

        private IEnumerable<AuthorizedAttribute> GetAuthorizedAttributes(IInvocation invocation)
        {
            return Attribute.GetCustomAttributes(
                invocation.MethodInvocationTarget, typeof(AuthorizedAttribute))
                .Cast<AuthorizedAttribute>();
        }
    }
}