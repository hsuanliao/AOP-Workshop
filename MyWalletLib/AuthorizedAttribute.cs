using System;

namespace MyWalletLib
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizedAttribute : Attribute
    {
        public UserType UserType { get; set; }
    }
}