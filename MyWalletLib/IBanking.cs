namespace MyWalletLib
{
    public interface IBanking
    {
        void Withdraw(string bankingAccount, decimal amount);
    }
}