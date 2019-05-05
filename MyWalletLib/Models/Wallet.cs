using System;
using System.Threading;

namespace MyWalletLib.Models
{
    //[Intercept(typeof(LogInterceptorInLib))]
    public class Wallet : IWallet
    {
        private readonly IBanking _bankingAdapter;
        private readonly IFee _feeAdapter;
        private readonly IWalletRepo _walletRepo;

        public Wallet(IWalletRepo walletRepo, IBanking bankingAdapter, IFee feeAdapter)
        {
            _walletRepo = walletRepo;
            _bankingAdapter = bankingAdapter;
            _feeAdapter = feeAdapter;
        }

        [Cache(Duration = 1000)]
        public string CreateGuid(string account, int token)
        {
            Console.WriteLine($"sleep 1.5 seconds, account:{account}, token:{token}");
            Thread.Sleep(1500);
            return Guid.NewGuid().ToString("N");
        }

        [LogParameters]
        [Authorized(UserType = UserType.VIP)]
        [Authorized(UserType = UserType.Guest)]
        public void Deposit(string bankingAccount, decimal amount, string account)
        {
            _bankingAdapter.Deposit(bankingAccount, amount);
            _walletRepo.UpdateDelta(account, amount);
        }

        [LogParameters]
        [Transaction(Role.DBA)]
        public void Withdraw(string account, decimal amount, string bankingAccount)
        {
            if (account.Equals("transaction error"))
            {
                throw new TransactionAbortedException();
            }

            _walletRepo.UpdateDelta(account, amount * -1);
            var fee = _feeAdapter.GetFee(account);
            _bankingAdapter.Withdraw(bankingAccount, amount - fee);
        }
    }
}