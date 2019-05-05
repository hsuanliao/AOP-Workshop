using System;
using MyWalletLib;
using MyWalletLib.Models;

namespace MyConsole
{
    internal class FakeBankingAdapter : IBanking
    {
        public void Deposit(string bankingAccount, decimal amount)
        {
            Console.WriteLine($"{nameof(Deposit)}({bankingAccount}, {amount})");
        }

        public void Withdraw(string bankingAccount, decimal amount)
        {
            Console.WriteLine($"{nameof(Withdraw)}({bankingAccount}, {amount})");
        }
    }

    internal class FakeFeeAdapter : IFee
    {
        public decimal GetFee(string account)
        {
            Console.WriteLine($"{nameof(GetFee)}({account})");
            return 10;
        }
    }

    internal class FakeWalletRepo : IWalletRepo
    {
        public void UpdateDelta(string account, decimal amount)
        {
            Console.WriteLine($"{nameof(UpdateDelta)}({account}, {amount})");
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var fakeWalletRepo = new FakeWalletRepo();
            var fakeBankingAdapter = new FakeBankingAdapter();
            var fakeFeeAdapter = new FakeFeeAdapter();

            var wallet = new Wallet(fakeWalletRepo, fakeBankingAdapter, fakeFeeAdapter);

            wallet.Deposit("joey", 1000, "123456789");
            wallet.Withdraw("joey", 1000, "123456789");
        }
    }
}