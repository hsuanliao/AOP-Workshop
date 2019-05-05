namespace MyWalletLib.Models
{
    public class LoggerDecorator : IWallet
    {
        private readonly ILogger _logger;
        private readonly IWallet _wallet;

        public LoggerDecorator(IWallet wallet, ILogger logger)
        {
            _wallet = wallet;
            _logger = logger;
        }

        public string CreateGuid(string account, int token)
        {
            return _wallet.CreateGuid(account, token);
        }

        public void Deposit(string bankingAccount, decimal amount, string account)
        {
            LoggerMessage($"{nameof(Deposit)}: {bankingAccount}-{amount}-{account}");
            _wallet.Deposit(bankingAccount, amount, account);
        }

        public void Withdraw(string account, decimal amount, string bankingAccount)
        {
            LoggerMessage($"{nameof(Withdraw)}: {bankingAccount}-{amount}-{account}");
            _wallet.Withdraw(account, amount, bankingAccount);
        }

        private void LoggerMessage(string message)
        {
            _logger.Info(message);
        }
    }
}