using MyWalletLib;
using MyWalletLib.Models;
using NSubstitute;
using NUnit.Framework;

namespace MyWalletLibTests
{
    [TestFixture]
    public class WalletTests
    {
        private IBanking _bankingAdapter;
        private Wallet _wallet;
        private IWalletRepo _walletRepo;

        [SetUp]
        public void Setup()
        {
            _walletRepo = Substitute.For<IWalletRepo>();
            _bankingAdapter = Substitute.For<IBanking>();

            _wallet = new Wallet(_walletRepo, _bankingAdapter);
        }

        [Test]
        public void withdrawal_from_wallet_to_banking_account_successfully()
        {
            _wallet.Withdraw("joey", 1000m, "123456789");

            _walletRepo.Received(1).UpdateDelta("joey", -1000);
            _bankingAdapter.Received(1).Withdraw("123456789", 1000);
        }
    }
}