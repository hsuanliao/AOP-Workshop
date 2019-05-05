namespace MyWalletLib.Models
{
    public interface IWallet
    {
        string CreateGuid(string account, int token);

        void Deposit(string bankingAccount, decimal amount, string account);

        void Withdraw(string account, decimal amount, string bankingAccount);
    }
}