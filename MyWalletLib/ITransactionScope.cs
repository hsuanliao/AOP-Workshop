using System;

namespace MyWalletLib
{
    public interface ITransactionScope : IDisposable
    {
        void Complete();

        void Dispose();
    }
}