namespace MyWalletLib.Models
{
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

        public void Deposit(string bankingAccount, decimal amount, string account)
        {
            _bankingAdapter.Deposit(bankingAccount, amount);
            _walletRepo.UpdateDelta(account, amount);
        }

        public void Withdraw(string account, decimal amount, string bankingAccount)
        {
            _walletRepo.UpdateDelta(account, amount * -1);
            var fee = _feeAdapter.GetFee(account);
            _bankingAdapter.Withdraw(bankingAccount, amount - fee);
        }
    }
}