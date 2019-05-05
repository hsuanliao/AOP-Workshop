namespace MyWalletLib.Models
{
    public class Wallet
    {
        private readonly IBanking _bankingAdapter;
        private readonly IWalletRepo _walletRepo;

        public Wallet(IWalletRepo walletRepo, IBanking bankingAdapter)
        {
            _walletRepo = walletRepo;
            _bankingAdapter = bankingAdapter;
        }

        public void Withdraw(string account, decimal amount, string bankingAccount)
        {
            _walletRepo.UpdateDelta(account, amount * -1);
            _bankingAdapter.Withdraw(bankingAccount, amount);
        }
    }
}