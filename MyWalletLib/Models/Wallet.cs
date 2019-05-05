namespace MyWalletLib.Models
{
    public class Wallet
    {
        private readonly IBanking _bankingAdapter;
        private readonly IFee _feeAdapter;
        private readonly ILogger _logger;
        private readonly IWalletRepo _walletRepo;

        public Wallet(IWalletRepo walletRepo, IBanking bankingAdapter, IFee feeAdapter, ILogger logger)
        {
            _walletRepo = walletRepo;
            _bankingAdapter = bankingAdapter;
            _feeAdapter = feeAdapter;
            _logger = logger;
        }

        public void Deposit(string bankingAccount, decimal amount, string account)
        {
            _logger.Info($"{nameof(Deposit)}: {bankingAccount}-{amount}-{account}");

            _bankingAdapter.Deposit(bankingAccount, amount);
            _walletRepo.UpdateDelta(account, amount);
        }

        public void Withdraw(string account, decimal amount, string bankingAccount)
        {
            _logger.Info($"{nameof(Withdraw)}: {bankingAccount}-{amount}-{account}");

            _walletRepo.UpdateDelta(account, amount * -1);
            var fee = _feeAdapter.GetFee(account);
            _bankingAdapter.Withdraw(bankingAccount, amount - fee);
        }
    }
}