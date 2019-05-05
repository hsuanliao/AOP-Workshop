using System;

namespace MyWalletLib
{
    public class TransactionAttribute : Attribute
    {
        public TransactionAttribute(Role role)
        {
            Role = role;
        }

        public Role Role { get; set; }
    }
}