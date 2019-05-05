namespace MyWalletLib
{
    public interface IBanking
    {
        void Deposit(string bankingAccount, decimal amount);

        void Withdraw(string bankingAccount, decimal amount);
    }
}