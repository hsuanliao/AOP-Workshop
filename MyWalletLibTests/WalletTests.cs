using MyWalletLib;
using MyWalletLib.Models;
using NSubstitute;
using NUnit.Framework;

namespace MyWalletLibTests
{
    [TestFixture]
    public class WalletTests
    {
        [Test]
        public void withdrawal_from_wallet_to_banking_account_successfully()
        {
            var walletRepo = Substitute.For<IWalletRepo>();
            var bankingAdapter = Substitute.For<IBanking>();

            var wallet = new Wallet(walletRepo, bankingAdapter);

            wallet.Withdraw("joey", 1000m, "123456789");

            walletRepo.Received(1).UpdateDelta("joey", -1000);
            bankingAdapter.Received(1).Withdraw("123456789", 1000);
        }
    }
}