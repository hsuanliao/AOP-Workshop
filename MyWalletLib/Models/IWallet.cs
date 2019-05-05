namespace MyWalletLib.Models
{
    public interface IWallet
    {
        void Deposit(string bankingAccount, decimal amount, string account);

        void Withdraw(string account, decimal amount, string bankingAccount);
    }
}